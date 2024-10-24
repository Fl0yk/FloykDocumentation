using Identity.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.EntityConfigurations;

public class FollowingEntityTypeConfigurator : IEntityTypeConfiguration<Following>
{
    public void Configure(EntityTypeBuilder<Following> builder)
    {
        builder.HasKey(f => new { f.UserId, f.AuthorId });

        builder
            .HasOne(f => f.Author)
            .WithMany()
            .HasForeignKey(f => f.AuthorId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
