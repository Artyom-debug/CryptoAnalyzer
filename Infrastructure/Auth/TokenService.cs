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
    private readonly IConfiguration _configuration;

    public TokenService(JwtOptions jwtOptions, IRefreshTokenRepository refreshToken, UserManager<ApplicationUser> manager, IConfiguration configuration)
    {
        _jwtOptions = jwtOptions;
        _refreshTokenRepository = refreshToken;
        _configuration = configuration;
        _userManager = manager;
    }

    private async Task<(string token, DateTime expiresAt)> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]!);
        var tokenValidity = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
        var tokenExpireTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidity);
        var signing = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        //var roles = await _identityService.GetUserRolesAsync(user);

        var stamp = await _userManager.GetSecurityStampAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("ss", stamp)
        };
        //claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r));

        var token = new JwtSecurityToken(issuer, audience, claims, DateTime.UtcNow, tokenExpireTimeStamp, signing);
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenValue, tokenExpireTimeStamp);
    }

    private (string token, RefreshToken entity) GenerateRefreshToken(string userId)
    {
        var refreshTokenValidity = _configuration.GetValue<int>("JwtConfig:RefreshTokenValidityDays");
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
        var tokenPair = await GenerateTokenPairAsync(userId, ct);
        await _refreshTokenRepository.RevokeTokenAsync(refresh, ct);
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
