using Core.Api.Extensions;
using Core.Api.Models.Options;
using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.Domain.Abstractions.Managers;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Entities;
using Identity.Infrastructure.Database;
using Identity.Presentation.Managers;
using Identity.Presentation.Providers;
using Identity.Presentation.Shared.Options.Setups;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace Identity.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        services.AddScoped<IImageManager, ImageManager>();

        services.AddTransient<IEmailManager, EmailManager>();

        services.ConfigureOptions();

        services.ConfigureAuthorization(configuration);

        services
            .AddIdentity<User, IdentityRole<Guid>>(opt => opt.User.RequireUniqueEmail = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>();

        services.AddControllers();

        services.AddFluentValidationAutoValidation();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddEndpointsApiExplorer();
        services.ConfigureSwaggerGen();

        services.ConfigureCors(configuration);

        return services;
    }

    private static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        // KEEP launchSettings.json and applicatoinSettings.json in sync
        services.ConfigureOptions<WWWRootOptionsSetup>();
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
                builder.AllowAnyOrigin()//WithOrigins(urls.ApiGatewayUrl, urls.ForumUrl, urls.ArticleUrl)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
}
