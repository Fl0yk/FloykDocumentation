using Article.Domain.Entities;
using Core.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EntityConfigurations;

internal sealed class SavedArticleEntityTypeConfigurator : BaseEntityTypeConfiguration<SavedArticle>, IEntityTypeConfiguration<SavedArticle>
{
    public override void Configure(EntityTypeBuilder<SavedArticle> builder)
    {
        builder.ToTable("saved_articles");

        base.Configure(builder);

        builder.Property(x => x.ArticleId)
            .IsRequired()
            .HasColumnName("article_id");

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.HasOne(x => x.User)
            .WithMany(x => x.SavedArticles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => new { x.IsDeleted, x.UserId, x.ArticleId })
            .HasFilter("is_deleted = false")
            .IsUnique();
    }
}
