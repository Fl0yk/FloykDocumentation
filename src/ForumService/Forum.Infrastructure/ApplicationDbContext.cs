using Forum.Domain.Entities;
using Forum.Infrastructure.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Question> Questions => Set<Question>();

    public DbSet<Answer> Answers => Set<Answer>();

    public DbSet<User> Users => Set<User>();

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnswerEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new  QuestionEntityTypeConfigurator());
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfigurator());

        base.OnModelCreating(modelBuilder);
    }
}
