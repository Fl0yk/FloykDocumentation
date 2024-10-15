using Forum.Domain.Entities;
using Forum.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.EntityConfigurations;

public class QuestionEntityTypeConfigurator : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.AuthorId).IsRequired();
        builder.Property(q => q.Title).IsRequired().HasMaxLength(250);
        builder.Property(q => q.DateOfCreation).IsRequired();
        builder.Property(q => q.Description).IsRequired();

        builder
            .HasMany(q => q.Answers)
            .WithOne()
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.SeedQuestion();
    }
}
