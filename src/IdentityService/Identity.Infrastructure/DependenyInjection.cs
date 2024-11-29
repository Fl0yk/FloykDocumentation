using Identity.Contracts.Abstractions.Services;
using Identity.Infrastructure.Consumers;
using Identity.Infrastructure.gRPC.Services.Clients;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Infrastructure;

public static class DependenyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.ConfigureMassTransit();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IArticleService, ArticleService>();
    }

    private static void ConfigureMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(conf =>
        {
            conf.SetKebabCaseEndpointNameFormatter();

            conf.AddConsumer<ArticleDeletedConsumer>();

            conf.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
