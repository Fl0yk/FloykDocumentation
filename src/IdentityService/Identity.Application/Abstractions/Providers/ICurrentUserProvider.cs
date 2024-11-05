using Identity.Application.Shared.Models;

namespace Identity.Application.Abstractions.Providers;

public interface ICurrentUserProvider
{
    public CurrentUser? GetCurrentUser();

    public CurrentUser? GetCurrentUser(string jwt);
}
