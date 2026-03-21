using Domain.Common;
namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string UserId { get; set; } = default!;
    public string TokenHash { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsRevoked { get; set; } = false;

    public RefreshToken(string userId, string tokenHash, DateTime createdAtUtc, DateTime expiresAtUtc)
    {
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
    }

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAtUtc;
}


