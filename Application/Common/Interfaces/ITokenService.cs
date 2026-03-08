using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    public Task<TokenPair> GenerateTokenPairAsync(string userId, CancellationToken ct);
    public Task<TokenPair> RefreshAsync(string refreshTokenValue, CancellationToken ct);
    public Task RevokeAllTokensAsync(string userId, CancellationToken ct);
}
