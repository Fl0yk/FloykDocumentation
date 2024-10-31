using Article.Domain.Abstractions.Repositories;
using Article.Infrastructure.Repositories;
using Article.Infrastructure.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
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

        MongoClient client = new(dbSettings.ConnectionString);
        var dataBase = client.GetDatabase(dbSettings.DatabaseName);

        services.AddSingleton(dataBase.GetCollection<ArticleDb>(dbSettings.ArticlesCollectionName));
        services.AddSingleton(dataBase.GetCollection<CategoryDb>(dbSettings.CategoriesCollectionName));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}