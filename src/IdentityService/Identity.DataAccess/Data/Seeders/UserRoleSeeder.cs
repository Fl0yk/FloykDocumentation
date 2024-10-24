using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.Seeders;

public static class UserRoleSeeder
{
    public static void SeedUserRole(this EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.HasData([
            new IdentityUserRole<Guid>()
            {
                UserId = Guid.Parse("ac2d055a-4d0f-41d2-90f9-88393f1b65e7"),
                RoleId = Guid.Parse("fe2d04aa-4d0f-41d2-90f9-88393f1b65e7")
            },
            new IdentityUserRole<Guid>()
            {
                UserId = Guid.Parse("ac2d055a-4d0f-41d2-90f9-88393f1b65e7"),
                RoleId = Guid.Parse("ba74ad5b-68da-4823-9a77-a424b56dac04")
            },
            new IdentityUserRole<Guid>()
            { 
                RoleId = Guid.Parse("ba74ad5b-68da-4823-9a77-a424b56dac04"),
                UserId = Guid.Parse("bb2d055a-4d0f-41d2-90f9-88393f1b65e7")
            }

        ]);
    }
}
