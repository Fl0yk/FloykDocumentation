using Article.Domain.Entities;
using Core.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EntityConfigurations;

internal sealed class UserEntityTypeConfigurator : BaseEntityTypeConfiguration<User>, IEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        base.Configure(builder);

        builder.Property(x => x.Username)
            .IsRequired()
            .HasColumnName("username");

        builder.Property(x => x.NormalizedUsername)
            .IsRequired()
            .HasColumnName("normalized_username");

        builder.Property(x => x.PublicUsername)
            .IsRequired()
            .HasColumnName("public_username");

        builder.HasIndex(x => new { x.IsDeleted, x.Username });
    }
}
