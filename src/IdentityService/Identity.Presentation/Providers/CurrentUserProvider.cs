using Identity.Application.Abstractions.Providers;
using Identity.Application.Shared.Models;
using System.Security.Claims;

namespace Identity.Presentation.Providers;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IJwtProvider _jwtProvider;

    public CurrentUserProvider(IHttpContextAccessor contextAccessor, IJwtProvider jwtProvider)
    {
        _contextAccessor = contextAccessor;
        _jwtProvider = jwtProvider;
    }

    public CurrentUser? GetCurrentUser()
    {
        var user = _contextAccessor?.HttpContext?.User;

        if (user is null)
        {
            throw new InvalidOperationException("User context is not present");
        }

        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        string id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        string email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        string username = user.FindFirst(c => c.Type == ClaimTypes.Name)!.Value;
        IEnumerable<string> roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        return new(Guid.Parse(id), email, username, roles);
    }

    public CurrentUser? GetCurrentUser(string jwt)
    {
        ClaimsPrincipal? principal = _jwtProvider.GetClaimsPrincipal(jwt);

        if (principal is null)
        {
            return null;
        }

        string id = principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        string email = principal.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        string username = principal.FindFirst(c => c.Type == ClaimTypes.Name)!.Value;
        IEnumerable<string> roles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        return new(Guid.Parse(id), email, username, roles);
    }
}
