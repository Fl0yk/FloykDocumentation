using Core.Models;
using Core.Providers.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Api.Providers.Implementations;
public class BaseCurrentUserProvider : IBaseCurrentUserProvider
{
    protected readonly IHttpContextAccessor _contextAccessor;

    public BaseCurrentUserProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
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
        string publicUsername = user.FindFirst(c => c.Type == "PublicUsername")!.Value;
        IEnumerable<string> roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        return new CurrentUser()
        {
            Id = Guid.Parse(id),
            Email = email,
            Username = username,
            PublicUsername = publicUsername,
            Roles = roles
        };
    }
}
