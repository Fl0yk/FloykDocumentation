using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Data.EntityConfigurations;

namespace Identity.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new FollowingEntityTypeConfigurator());
        builder.ApplyConfiguration(new RoleEntityTypeConfigurator());
        builder.ApplyConfiguration(new SavedArticleEntityTypeConfigurator());
        builder.ApplyConfiguration(new UserEntityTypeConfigurator());
    }
}