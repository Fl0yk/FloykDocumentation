using Core.Infrastructure.EntityConfigurations;
using Identity.DataAccess.Data.Seeders;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.EntityConfigurations;

internal class UserEntityTypeConfigurator : BaseEntityTypeConfiguration<User>, IEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        base.Configure(builder);

        builder.Property(x => x.UserName)
            .IsRequired()
            .HasColumnName("username");

        builder.Property(x => x.PublicUsername)
            .IsRequired()
            .HasColumnName("public_username");

        builder.Property(x => x.NormalizedUserName)
            .IsRequired()
            .HasColumnName("normalized_username");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("email");

        builder.Property(x => x.EmailConfirmed)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("email_confirmed");

        builder.Property(x => x.Avatar)
            .IsRequired(false)
            .HasColumnName("avatar");

        builder.Property(x => x.RefreshToken)
            .IsRequired(false)
            .HasColumnName("refresh_token");

        builder.Property(x => x.AccessFailedCount)
            .IsRequired()
            .HasColumnName("access_failed_count");

        builder.Property(x => x.ConcurrencyStamp)
            .IsRequired(false)
            .HasColumnName("concurrencyStamp");

        builder.Property(x => x.LockoutEnabled)
            .IsRequired()
            .HasColumnName("lockout_enambled");

        builder.Property(x => x.LockoutEnd)
            .IsRequired(false)
            .HasColumnName("lockout_end");

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasColumnName("password_hash");

        builder.Property(x => x.RefreshToken)
            .IsRequired(false)
            .HasColumnName("refresh_token");

        builder.Property(x => x.RefreshTokenExpiry)
            .IsRequired(false)
            .HasColumnName("refresh_token_expiry");

        builder.Property(x => x.SecurityStamp)
            .IsRequired(false)
            .HasColumnName("security_stamp");

        builder.HasMany(u => u.Followings)
            .WithOne(x => x.User)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => new { x.IsDeleted, x.NormalizedUserName })
            .HasFilter("is_deleted = false")
            .IsUnique();

        builder.HasIndex(x => new { x.IsDeleted, x.PublicUsername });

        builder.Ignore(x => x.PhoneNumber);
        builder.Ignore(x => x.PhoneNumberConfirmed);
        builder.Ignore(x => x.TwoFactorEnabled);

        builder.SeedUsers();
    }
}
