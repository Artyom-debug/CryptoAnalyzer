namespace Infrastructure.Auth;

public class JwtOptions
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public int TokenValidityMins { get; set; }
    public int RefreshTokenValidityDays { get; set; }
}
