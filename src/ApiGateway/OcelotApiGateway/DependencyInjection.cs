using Core.Api.Extensions;
using Core.Api.Models.Options;
using Ocelot.DependencyInjection;
using OcelotApiGateway.DelegatingHandlers;

namespace OcelotApiGateway;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddOcelot(configuration)
                .AddDelegatingHandler<JwtInjectionHandler>();

        services.AddSwaggerForOcelot(configuration);

        services.ConfigureAuthorization(configuration);

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}
