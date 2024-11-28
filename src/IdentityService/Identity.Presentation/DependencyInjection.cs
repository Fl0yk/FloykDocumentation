using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.Application.Abstractions.Managers;
using Identity.Application.Abstractions.Providers;
using Identity.DataAccess.Data;
using Identity.DataAccess.Entities;
using Identity.Presentation.Managers;
using Identity.Presentation.Options.Models;
using Identity.Presentation.Options.Setups;
using Identity.Presentation.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;
using System.Text;

namespace Identity.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        services.AddScoped<IImageManager, ImageManager>();

        services.ConfigureOptions();

        services.ConfigureAuthorization(configuration);

        services.ConfigureSerilog(configuration);

        services
            .AddIdentity<User, IdentityRole<Guid>>(opt => opt.User.RequireUniqueEmail = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddControllers();

        services.AddFluentValidationAutoValidation();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

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

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    private static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        // KEEP launchSettings.json and applicatoinSettings.json in sync
        services.ConfigureOptions<WWWRootOptionsSetup>();
        services.ConfigureOptions<JwtOptionsSetup>();

        return services;
    }

    private static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        JwtOptions jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>()
                                    ?? throw new KeyNotFoundException("Can't read jwt from appsettings.json");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateActor = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtOptions.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

            };
        });

        services.AddAuthorization();
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
