using Article.Domain.Abstractions.Repositories;
using Article.Infrastructure.Consumers.Users;
using Article.Infrastructure.Data;
using Article.Infrastructure.Data.TransactionProviders;
using Article.Infrastructure.Database;
using Article.Infrastructure.Repositories;
using Core.Infrastructure.DataBase;
using Core.Providers.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Reflection;

namespace Article.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        DocumentationArticleDbSettings dbSettings = configuration
            .GetSection("DocumentationDatabaseSettings")
            .Get<DocumentationArticleDbSettings>() ?? throw new ArgumentNullException("Settings for mongodb was not found");

        var dbContext = new ApplicationDbContext(dbSettings);

        services.AddSingleton(dbContext.ArticleCollection);

        services.AddScoped<ISaveChangesInterceptor, DatabaseAuditableInterceptor>();
        services.AddScoped<ITransactionProvider, SqlTransactionProvider>();
        //TODO: mongo db transactions
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<SqlDbContext>((sp, cfg) => {
            cfg.UseNpgsql(dbSettings.SqlConnectionString);
            cfg.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
        });

        services.ConfigureMassTransit("article");

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }

    private static void ConfigureMassTransit(this IServiceCollection services, string prefix)
    {
        services.AddMassTransit(conf =>
        {
            conf.SetKebabCaseEndpointNameFormatter();

            conf.AddConsumer<UserCreatedEventConsumer>();
            conf.AddConsumer<UserUpdatedEventConsumer>();

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