using Core.Infrastructure.DataBase;
using Core.Providers.Interfaces;
using Identity.DataAccess.Data;
using Identity.DataAccess.Repositories.Implementations;
using Identity.Domain.Repositories.Abstractions;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
                                                       ?? throw new ArgumentNullException("Connection string is not found");

        services.AddScoped<ISaveChangesInterceptor, DatabaseAuditableInterceptor>();
        services.AddScoped<ITransactionProvider, TransactionProvider>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<ApplicationDbContext>((sp, cfg) => {
            cfg.UseNpgsql(connectionString);
            cfg.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
        });

        return services;
    }
}
