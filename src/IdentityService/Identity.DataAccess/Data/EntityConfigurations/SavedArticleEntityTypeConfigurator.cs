using Identity.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.EntityConfigurations;

public class SavedArticleEntityTypeConfigurator : IEntityTypeConfiguration<SavedArticle>
{
    public void Configure(EntityTypeBuilder<SavedArticle> builder)
    {
        builder.HasKey(s => new { s.UserId, s.ArticleId });

        builder.Property(s => s.ArticleName).IsRequired();
    }
}
