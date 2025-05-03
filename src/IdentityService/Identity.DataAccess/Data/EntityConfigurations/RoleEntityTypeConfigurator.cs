using Identity.DataAccess.Data.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Data.EntityConfigurations;

public class RoleEntityTypeConfigurator : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("name");

        builder.Property(x => x.NormalizedName)
            .IsRequired()
            .HasColumnName("normalized_name");

        builder.SeedRoles();
    }
}
