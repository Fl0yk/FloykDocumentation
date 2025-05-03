using Core.Models;
using Core.Providers.Interfaces;

namespace Identity.Domain.Abstractions.Providers;

public interface ICurrentUserProvider : IBaseCurrentUserProvider
{
    CurrentUser? GetCurrentUser(string jwt);
}
