using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigration<TDbContext>(this IServiceScope applicationScope)
        where TDbContext : DbContext
    {
        using TDbContext context = applicationScope.ServiceProvider.GetRequiredService<TDbContext>();

        context.Database.Migrate();
    }
}
