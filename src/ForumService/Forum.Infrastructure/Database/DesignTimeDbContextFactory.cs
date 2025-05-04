using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Database;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        return new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Host=localhost;Port=7004;Database=FloykDocumentation.Forum;Username=postgres;Password=postgres")
                .Options
        );
    }
}
