using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IApplicationDbContext _context;

    public RefreshTokenRepository(IApplicationDbContext context) =>
        _context = context;

    public async Task StoreRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellation)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(cancellation);
    }

    public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string hash)
    {
        var token = await _context.RefreshTokens
            .AsTracking()
            .Where(r => r.TokenHash == hash)
            .FirstOrDefaultAsync();
        return token;
    }
    
    public async Task RevokeTokenAsync(RefreshToken refresh, CancellationToken cancellation)
    {
        refresh.IsRevoked = true;
        await _context.SaveChangesAsync(cancellation);
    }

    public async Task RevokeAllRefreshAsync(string userId, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var tokens = await _context.RefreshTokens
            .Where(r => r.UserId == userId && !r.IsRevoked && r.ExpiresAtUtc > now)
            .ToListAsync(ct);

        foreach(var token in tokens)
        {
            token.IsRevoked = true;
        }
        await _context.SaveChangesAsync(ct);
    }
}
