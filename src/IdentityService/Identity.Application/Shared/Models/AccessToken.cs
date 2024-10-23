namespace Identity.Application.Shared.Models;

public class AccessToken
{
    public required string JwtToken { get; set; }

    public required string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiry { get; set; }
}
