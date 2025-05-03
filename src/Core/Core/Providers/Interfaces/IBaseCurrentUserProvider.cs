using Core.Models;

namespace Core.Providers.Interfaces;

public interface IBaseCurrentUserProvider
{
    public CurrentUser? GetCurrentUser();
}
