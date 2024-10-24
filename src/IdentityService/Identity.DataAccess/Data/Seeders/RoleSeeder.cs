using Identity.DataAccess.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.Seeders;

public static class RoleSeeder
{
    public static void SeedRoles(this EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.HasData([
            new(Roles.Author)
            {
                Id = Guid.Parse("fe2d04aa-4d0f-41d2-90f9-88393f1b65e7"),
                NormalizedName = Roles.Author.ToUpper()
            },
            new(Roles.Admin)
            {
                Id = Guid.Parse("ba74ad5b-68da-4823-9a77-a424b56dac04"),
                NormalizedName= Roles.Admin.ToUpper()
            }]);
    }
}
