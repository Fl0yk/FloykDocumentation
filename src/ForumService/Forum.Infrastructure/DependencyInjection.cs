using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Forum.Domain.Abstractions.Repositories;
using Forum.Infrastructure.Repositories;

namespace Forum.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                                IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
                                                        ?? throw new ArgumentNullException("Connection string is not found");

        services.AddDbContext<ApplicationDbContext>(cfg => cfg.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
