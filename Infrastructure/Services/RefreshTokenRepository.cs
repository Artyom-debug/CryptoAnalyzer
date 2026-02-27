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
}
