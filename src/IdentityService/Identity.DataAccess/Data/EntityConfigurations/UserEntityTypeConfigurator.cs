using Identity.DataAccess.Data.Seeders;
using Identity.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.EntityConfigurations;

internal class UserEntityTypeConfigurator : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(u => u.SavedArticles)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

        builder.HasMany(u => u.Followings)
            .WithOne()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.SeedUsers();
    }
}
