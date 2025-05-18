using Article.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.Seeders;

public static class UserSeeder
{
    public static void SeedUsers(this EntityTypeBuilder<User> builder)
    {
        var admin = new User()
        {
            Id = Guid.Parse("ac2d055a-4d0f-41d2-90f9-88393f1b65e7"),
            Username = "Admin",
            PublicUsername = "Admin",
            NormalizedUsername = "ADMIN",
        };

        var author = new User()
        {
            Id = Guid.Parse("bb2d055a-4d0f-41d2-90f9-88393f1b65e7"),
            Username = "Author",
            PublicUsername = "Author",
            NormalizedUsername = "AUTHOR",
        };

        var client = new User()
        {
            Id = Guid.Parse("ff2d055a-4d0f-41d2-90f9-88393f1b65e7"),
            Username = "Floyk",
            PublicUsername = "Floyk",
            NormalizedUsername = "FLOYK",
        };

        builder.HasData(admin, author, client);
    }
}
