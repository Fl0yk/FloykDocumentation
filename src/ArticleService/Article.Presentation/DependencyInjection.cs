using Article.Infrastructure.Consumers.User;
using Article.Infrastructure.Options.Models;
using Article.Presentation.Shared.Options.Setups;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Article.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions();

        services.AddControllers();

        services.AddFluentValidationAutoValidation();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.ConfigureMassTransit();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
        });

        services.ConfigureCors(configuration);

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    private static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        // KEEP launchSettings.json and applicatoinSettings.json in sync
        services.ConfigureOptions<UrlsOptionSetup>();

        return services;
    }

    private static void ConfigureMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(conf =>
        {
            conf.SetKebabCaseEndpointNameFormatter();

            conf.AddConsumer<UsernameUpdatedConsumer>();

            conf.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }

    private static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        UrlsOption urls = configuration.GetSection("Urls").Get<UrlsOption>()
                                            ?? throw new KeyNotFoundException("Can't read urls from appsettings.json");

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(urls.ApiGatewayUrl, urls.ForumUrl, urls.IdentityUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}
