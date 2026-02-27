using Domain.Entities;

namespace Infrastructure.Repository;

public interface IRefreshTokenRepository
{
    public Task StoreRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellation);

}
