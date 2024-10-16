using Forum.Domain.Entities;
using Forum.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Question> Questions => Set<Question>();

    public DbSet<Answer> Answers => Set<Answer>();

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnswerEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new  QuestionEntityTypeConfigurator());

        base.OnModelCreating(modelBuilder);
    }
}
