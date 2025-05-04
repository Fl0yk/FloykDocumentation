using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Article.Infrastructure.Database;

namespace Article.Infrastructure.Database;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlDbContext>
{
    public SqlDbContext CreateDbContext(string[] args)
    {
        return new SqlDbContext(
            new DbContextOptionsBuilder<SqlDbContext>()
                .UseNpgsql("Host=localhost;Port=5004;Database=FloykDocumentation.Article;Username=postgres;Password=postgres")
                .Options
        );
    }
}
