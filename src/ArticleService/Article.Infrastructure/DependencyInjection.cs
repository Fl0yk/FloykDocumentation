using Article.Domain.Abstractions.Repositories;
using Article.Infrastructure.Repositories;
using Article.Infrastructure.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Article.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        DocumentationArticleDbSettings dbSettings = configuration.GetValue<DocumentationArticleDbSettings>("DocumentationDatabaseSettings")
                                                                                ?? throw new ArgumentNullException("Settings for mongodb was not found");

        MongoClient client = new(dbSettings.ConnectionString);
        var dataBase = client.GetDatabase(dbSettings.DatabaseName);

        services.AddSingleton(dataBase.GetCollection<ArticleDb>(dbSettings.ArticlesCollectionName));
        services.AddSingleton(dataBase.GetCollection<CategoryDb>(dbSettings.CategoriesCollectionName));

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICatergoryRepository, CategoryRepository>();

        return services;
    }
}