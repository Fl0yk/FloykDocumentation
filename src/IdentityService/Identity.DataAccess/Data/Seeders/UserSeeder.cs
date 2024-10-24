using Identity.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.Seeders;

public static class UserSeeder
{
    public static void SeedUsers(this EntityTypeBuilder<User> builder)
    {
        var admin = new User()
        {
            Id = Guid.Parse("ac2d055a-4d0f-41d2-90f9-88393f1b65e7"),
            UserName = "Admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@mail.ru",
            NormalizedEmail = "ADMIN@MAIL.RU",
            SecurityStamp = Guid.NewGuid().ToString("D"),
            EmailConfirmed = true
        };

        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        admin.PasswordHash = passwordHasher.HashPassword(admin, "Qwerty123$");

        var author = new User()
        {
            Id = Guid.Parse("bb2d055a-4d0f-41d2-90f9-88393f1b65e7"),
            UserName = "Author",
            NormalizedUserName = "AUTHOR",
            Email = "aauthor@mail.ru",
            NormalizedEmail = "AUTHOR@MAIL.RU",
            SecurityStamp = Guid.NewGuid().ToString("D"),
            EmailConfirmed = true
        };

        author.PasswordHash = passwordHasher.HashPassword(author, "Qwerty123$");

        var client = new User()
        {
            Id = Guid.Parse("ff2d055a-4d0f-41d2-90f9-88393f1b65e7"),
            UserName = "Floyk",
            NormalizedUserName = "FLOYK",
            Email = "kosach@mail.ru",
            NormalizedEmail = "KOSACH@MAIL.RU",
            SecurityStamp = Guid.NewGuid().ToString("D"),
            EmailConfirmed = true
        };

        client.PasswordHash = passwordHasher.HashPassword(client, "Qwerty123$");

        builder.HasData(admin, author, client);
    }
}
