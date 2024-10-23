using Identity.DataAccess.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.Seeders;

public static class RoleSeeder
{
    public static void SeedRoles(this EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData([
            new(Roles.Author)
            {
                NormalizedName = Roles.Author.ToUpper()
            },
            new(Roles.Admin)
            {
                NormalizedName= Roles.Admin.ToUpper()
            }]);
    }
}
