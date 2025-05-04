using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        return new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Host=localhost;Port=6004;Database=FloykDocumentation.Identity;Username=postgres;Password=postgres")
                .Options
        );
    }
}
