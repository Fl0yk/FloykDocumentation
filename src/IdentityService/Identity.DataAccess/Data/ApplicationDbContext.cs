using Identity.DataAccess.Data.EntityConfigurations;
using Identity.DataAccess.Data.Seeders;
using Identity.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new FollowingEntityTypeConfigurator());
        builder.ApplyConfiguration(new RoleEntityTypeConfigurator());
        builder.ApplyConfiguration(new SavedArticleEntityTypeConfigurator());
        builder.ApplyConfiguration(new UserEntityTypeConfigurator());

        builder.Entity<IdentityUserRole<Guid>>().SeedUserRole();
    }
}