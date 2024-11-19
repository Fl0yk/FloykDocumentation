using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Article.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddFluentValidationAutoValidation();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.ConfigureSerilog(configuration);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
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
