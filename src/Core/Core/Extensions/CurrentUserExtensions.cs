using Core.Constants;
using Core.Models;

namespace Core.Extensions;

public static class CurrentUserExtensions
{
    public static bool IsAdmin(this CurrentUser user)
    {
        return user.Roles.Any(x => x == Roles.Admin);
    }

    public static bool IsAuthor(this CurrentUser user)
    {
        return user.Roles.Any(x => x == Roles.Author);
    }
}
