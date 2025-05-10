using Article.Presentation.Shared.Options.Setups;
using Core.Api.Extensions;
using Core.Api.Models.Options;
using Core.Api.Providers.Implementations;
using Core.Providers.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
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

        services.ConfigureAuthorization(configuration);
        services.AddHttpContextAccessor();

        services.AddScoped<IBaseCurrentUserProvider, BaseCurrentUserProvider>();

        services.AddEndpointsApiExplorer();
        services.ConfigureSwaggerGen();

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
                builder.AllowAnyOrigin()//.WithOrigins(urls.ApiGatewayUrl, urls.ForumUrl, urls.IdentityUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return;
    }
}
