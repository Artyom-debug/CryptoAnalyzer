using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Repository;

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
}
