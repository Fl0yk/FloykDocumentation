using Article.Domain.Abstractions.Repositories;
using Article.Domain.Abstractions.Services;
using Article.Infrastructure.Data;
using Article.Infrastructure.gRPC.Services;
using Article.Infrastructure.Repositories;
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

        ApplicationDbContext dbContext = new(dbSettings);

        services.AddSingleton(dbContext.ArticleCollection);
        services.AddSingleton(dbContext.CategoryCollection);

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserService, UserService>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}