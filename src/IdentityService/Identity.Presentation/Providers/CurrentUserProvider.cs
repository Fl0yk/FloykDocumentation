using Core.Api.Providers.Implementations;
using Core.Models;
using Identity.Domain.Abstractions.Providers;
using System.Security.Claims;

namespace Identity.Presentation.Providers;

public class CurrentUserProvider : BaseCurrentUserProvider, ICurrentUserProvider
{
    private readonly IJwtProvider _jwtProvider;

    public CurrentUserProvider(
        IHttpContextAccessor contextAccessor, 
        IJwtProvider jwtProvider) : base(contextAccessor)
    {
        _jwtProvider = jwtProvider;
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

        return new CurrentUser()
        {
            Id = Guid.Parse(id),
            Email = email,
            Username = username,
            Roles = roles
        };
    }
}
