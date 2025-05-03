using Identity.Domain.Entities;
using System.Security.Claims;

namespace Identity.Domain.Abstractions.Providers;

public interface IJwtProvider
{
    public string GenerateJwt(User user, IEnumerable<Claim> claims);

    public string GenerateRefreshToken();

    public ClaimsPrincipal? GetClaimsPrincipal(string token);
}
