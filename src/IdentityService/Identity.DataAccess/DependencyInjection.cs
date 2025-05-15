using Core.Infrastructure.DataBase;
using Core.Providers.Interfaces;
using Identity.DataAccess.Repositories.Implementations;
using Identity.Domain.Repositories.Abstractions;
using Identity.Infrastructure.Consumers.Articles;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Database;
using MassTransit;
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

        services.ConfigureMassTransit("identity");

        services.AddDbContext<ApplicationDbContext>((sp, cfg) => {
            cfg.UseNpgsql(connectionString);
            cfg.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
        });

        return services;
    }

    private static void ConfigureMassTransit(this IServiceCollection services, string prefix)
    {
        services.AddMassTransit(conf =>
        {
            conf.SetKebabCaseEndpointNameFormatter();

            conf.AddConsumer<ArticleApprovedEventConsumer>();

            conf.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(prefix, includeNamespace: false));

                cfg.Host("rabbitmq", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
