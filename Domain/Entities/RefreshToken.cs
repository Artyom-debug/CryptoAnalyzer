using Domain.Common;
namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string UserId { get; set; } = default!;
    public string TokenHash { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsRevoked { get; set; }

    public RefreshToken(string userId, string tokenHash, DateTime createdAtUtc, DateTime expiresAtUtc, bool isRevoked)
    {
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
        IsRevoked = isRevoked;
    }

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAtUtc;
    public bool IsActive(DateTime utcNow) => !IsRevoked && !IsExpired(utcNow);
}


