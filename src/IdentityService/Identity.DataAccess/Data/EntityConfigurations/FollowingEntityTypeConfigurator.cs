using Core.Infrastructure.EntityConfigurations;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.EntityConfigurations;

public class FollowingEntityTypeConfigurator : BaseEntityTypeConfiguration<Following>, IEntityTypeConfiguration<Following>
{
    public override void Configure(EntityTypeBuilder<Following> builder)
    {
        builder.ToTable("followings");

        base.Configure(builder);

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.AuthorId)
            .HasColumnName("author_id");

        builder
            .HasOne(f => f.Author)
            .WithMany()
            .HasForeignKey(f => f.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Followings)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(f => new { f.IsDeleted, f.UserId, f.AuthorId });
    }
}
