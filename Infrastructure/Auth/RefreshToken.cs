using Domain.Common;

namespace Infrastructure.Auth;

public class RefreshToken : BaseEntity
{
    public string UserId { get; set; } = default!;
    public string TokenHash { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsRevoked { get; set; }

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAtUtc;
    public bool IsActive(DateTime utcNow) => !IsRevoked && !IsExpired(utcNow);
}
