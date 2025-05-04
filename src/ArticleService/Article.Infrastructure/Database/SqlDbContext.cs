using Article.Domain.Entities;
using Article.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Article.Infrastructure.Database;

public sealed class SqlDbContext : DbContext
{
    internal DbSet<Category> Categories => Set<Category>();

    internal DbSet<User> Users => Set<User>();

    internal DbSet<SavedArticle> SavedArticles => Set<SavedArticle>();

    public SqlDbContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new SavedArticleEntityTypeConfigurator());

        base.OnModelCreating(modelBuilder);
    }
}