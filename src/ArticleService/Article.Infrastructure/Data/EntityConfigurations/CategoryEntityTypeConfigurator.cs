using Article.Domain.Entities;
using Article.Infrastructure.Data.Seeders;
using Core.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EntityConfigurations;

internal sealed class CategoryEntityTypeConfigurator : BaseEntityTypeConfiguration<Category>, IEntityTypeConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        base.Configure(builder);

        builder.Property(x => x.ParentId)
            .IsRequired(false)
            .HasColumnName("parent_id");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("name");

        builder.Property(x => x.Order)
            .IsRequired()
            .HasColumnName("order");

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.ChildCategories)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.SeedCategories();
    }
}
