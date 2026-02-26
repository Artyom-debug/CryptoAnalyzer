using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    public Task<TokenPair> GenerateTokenPairAsync(string userId, CancellationToken ct);
    public Task<TokenPair> RefreshAsync(string userId, CancellationToken ct);

}
