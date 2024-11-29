using FluentValidation;
using FluentValidation.AspNetCore;
using Forum.Infrastructure.Options.Models;
using Forum.Presentation.Options.Setups;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Forum.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentetionServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions();

        services.AddControllers();

        services.AddFluentValidationAutoValidation();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.ConfigureSerilog(configuration);

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

    private static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        UrlsOption urls = configuration.GetSection("Urls").Get<UrlsOption>()
                                            ?? throw new KeyNotFoundException("Can't read urls from appsettings.json");

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(urls.ApiGatewayUrl, urls.ArticleUrl, urls.IdentityUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    private static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        string elasticsearchUrl = configuration.GetSection("ElasticsearchUrl").Value
                                                    ?? throw new KeyNotFoundException("Can't read jwt from appsettings.json");

        ElasticsearchSinkOptions elasticsearchOptions = new(new Uri(elasticsearchUrl))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        };

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .WriteTo.Console()
            .WriteTo.Elasticsearch(elasticsearchOptions).MinimumLevel
                    .Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .CreateLogger();


        return services;
    }
}
