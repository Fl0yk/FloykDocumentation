using Core.Infrastructure.EntityConfigurations;
using Forum.Domain.Entities;
using Forum.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.EntityConfigurations;
public class AnswerEntityTypeConfigurator : BaseEntityTypeConfiguration<Answer>, IEntityTypeConfiguration<Answer>
{
    public override void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("answers");

        base.Configure(builder);

        builder.Property(a => a.Text)
            .IsRequired()
            .HasColumnName("text");

        builder.Property(x => x.Level)
            .IsRequired()
            .HasColumnName("level");

        builder.Property(x => x.AuthorId)
            .HasColumnName("author_id");

        builder.Property(x => x.ParentId)
            .HasColumnName("parent_id");

        builder.HasOne(a => a.Parent)
            .WithMany(a => a.Childrens)
            .HasForeignKey(a => a.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Question)
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        //TODO: return seeds
        //builder.SeedAnswer();
    }
}
