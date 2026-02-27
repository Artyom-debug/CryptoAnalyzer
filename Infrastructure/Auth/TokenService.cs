using Application.Common.Interfaces;
using Application.Common.Models;
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
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _configuration;

    public TokenService(JwtOptions jwtOptions, IRefreshTokenRepository refreshToken, IIdentityService identityService, IConfiguration configuration)
    {
        _jwtOptions = jwtOptions;
        _refreshTokenRepository = refreshToken;
        _identityService = identityService;
        _configuration = configuration;
    }

    private async Task<(string token, DateTime expiresAt)> GenerateAccessTokenAsync(IdentityUser user)
    {
        var issuer = _configuration["JwtConfig:Issuer"];
        var audience = _configuration["JwtConfig:Audience"];
        var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]!);
        var tokenValidity = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
        var tokenExpireTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidity);
        var signing = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        
        //var roles = await _identityService.GetUserRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        //claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r));

        var token = new JwtSecurityToken(issuer, audience, claims, DateTime.UtcNow, tokenExpireTimeStamp, signing);
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenValue, tokenExpireTimeStamp);
    }

    private (string token, RefreshToken entity) GenerateRefreshTokenAsync(string userId)
    {
        var refreshTokenValidity = _configuration.GetValue<int>("JwtConfig:RefreshTokenValidityDays");
        var tokenExpireTimeStamp = DateTime.UtcNow.AddDays(refreshTokenValidity);
        var bytes = RandomNumberGenerator.GetBytes(64);
        var plain = Base64(bytes);
        var hash = Sha256Hex(plain);

        var entity = new RefreshToken(userId, );
        return (plain, entity);
    }

    public async Task<TokenPair> GenerateTokenPairAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _identityService.GetUserById(userId) 
            ?? throw new UnauthorizedAccessException("User not found");
        var access = await GenerateAccessTokenAsync(user);
        var refresh = GenerateRefreshTokenAsync(user);

        await _refreshTokenRepository.StoreRefreshTokenAsync(refresh.entity, cancellationToken);

        return new TokenPair
        {
            AccessToken = access.token,
            RefreshToken = refresh.token,
            AccessTokenExpiresAt = access.expiresAt,
            RefreshTokenExpiresAt = refresh.entity.ExpiresAtUtc
        };
    }
}
