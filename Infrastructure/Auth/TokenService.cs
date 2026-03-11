using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.GuardClauses;
using Domain.Entities;
using Infrastructure.Repository;
using Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Auth;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(JwtOptions jwtOptions, IRefreshTokenRepository refreshToken, UserManager<ApplicationUser> manager)
    {
        _jwtOptions = jwtOptions;
        _refreshTokenRepository = refreshToken;
        _userManager = manager;
    }

    private async Task<(string token, DateTime expiresAt)> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
        var tokenExpireTimeStamp = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenValidityMins);
        var signing = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var stamp = await _userManager.GetSecurityStampAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("ss", stamp)
        };
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer, _jwtOptions.Audience,
            claims, DateTime.UtcNow, tokenExpireTimeStamp, signing);

        return (new JwtSecurityTokenHandler().WriteToken(token), tokenExpireTimeStamp);
    }

    private (string token, RefreshToken entity) GenerateRefreshToken(string userId)
    {
        var refreshTokenValidity = _jwtOptions.RefreshTokenValidityDays;
        var tokenExpireTimeStamp = DateTime.UtcNow.AddDays(refreshTokenValidity);
        var bytes = RandomNumberGenerator.GetBytes(64);
        var plain = Base64Url(bytes);
        var hash = Sha256Hex(plain);

        var entity = new RefreshToken(userId, hash, DateTime.UtcNow, tokenExpireTimeStamp);
        return (plain, entity);
    }

    public async Task<TokenPair> GenerateTokenPairAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId) 
            ?? throw new UnauthorizedAccessException("User not found");
        var access = await GenerateAccessTokenAsync(user);
        var refresh = GenerateRefreshToken(userId);

        await _refreshTokenRepository.StoreRefreshTokenAsync(refresh.entity, cancellationToken);

        return new TokenPair
        {
            AccessToken = access.token,
            RefreshToken = refresh.token,
            AccessTokenExpiresAt = access.expiresAt,
            RefreshTokenExpiresAt = refresh.entity.ExpiresAtUtc
        };
    }

    public async Task<TokenPair> RefreshAsync(string refreshTokenValue, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        string refreshTokenHash = Sha256Hex(refreshTokenValue); 
        var refresh = await _refreshTokenRepository.GetRefreshTokenByHashAsync(refreshTokenHash) ?? 
            throw new UnauthorizedAccessException("Invalid refresh.");

        if (refresh.IsRevoked)
        {
            await _refreshTokenRepository.RevokeAllRefreshAsync(refresh.UserId, ct);
            throw new UnauthorizedAccessException("Reuse detected.");
        }

        if (refresh.IsExpired(now))
            throw new UnauthorizedAccessException("Refresh token expired.");

        var userId = refresh.UserId;
        await _refreshTokenRepository.RevokeTokenAsync(refresh, ct);
        var tokenPair = await GenerateTokenPairAsync(userId, ct);
        return tokenPair;
    }

    public async Task RevokeAllTokensAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        Guard.Against.Null(user);
        await _refreshTokenRepository.RevokeAllRefreshAsync(userId, cancellationToken);
        await _userManager.UpdateSecurityStampAsync(user);
    }

    private static string Sha256Hex(string input)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
    private static string Base64Url(byte[] data)
        => Convert.ToBase64String(data).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
