using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity.Presentation.Providers;

public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    public UserClaimsPrincipalFactory(
    UserManager<User> userManager,
    IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        //Get the data from EF core
        var roles = await UserManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        identity.AddClaim(new Claim("PublicUsername", user.PublicUsername));
        identity.AddClaim(new Claim("roles", string.Join(',', identity.FindAll(ClaimTypes.Role).Select(x => x.Value))));

        return identity;
    }
}
