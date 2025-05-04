using Core.Infrastructure.EntityConfigurations;
using Forum.Domain.Entities;
using Forum.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.EntityConfigurations;

public class QuestionEntityTypeConfigurator : BaseEntityTypeConfiguration<Question>, IEntityTypeConfiguration<Question>
{
    public override void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("questions");

        base.Configure(builder);

        builder.Property(q => q.AuthorId)
            .IsRequired()
            .HasColumnName("author_id");

        builder.Property(q => q.Title)
            .IsRequired()
            .HasMaxLength(250)
            .HasColumnName("title");

        builder.Property(q => q.Description)
            .IsRequired()
            .HasColumnName("description");

        builder
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        //TODO: return seeds
        //builder.SeedQuestion();
    }
}
