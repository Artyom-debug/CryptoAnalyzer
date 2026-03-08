using Domain.Entities;

namespace Infrastructure.Repository;

public interface IRefreshTokenRepository
{
    public Task StoreRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellation);
    public Task<RefreshToken?> GetRefreshTokenByHashAsync(string hash);
    public Task RevokeTokenAsync(RefreshToken refresh, CancellationToken cancellation);
    public Task RevokeAllRefreshAsync(string userId, CancellationToken cancellation);
}
