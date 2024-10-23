using Identity.DataAccess.Entities;
using System.Security.Claims;

namespace Identity.Application.Abstractions.Providers;

public interface IJwtProvider
{
    public string GenerateJwt(User user, IEnumerable<Claim> claims);

    public string GenerateRefreshToken();

    public ClaimsPrincipal? GetClaimsPrincipal(string token);
}
