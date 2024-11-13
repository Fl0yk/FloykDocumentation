using Article.Infrastructure.Data.Seeders;
using Article.Infrastructure.Shared.Models;
using MongoDB.Driver;

namespace Article.Infrastructure.Data;

public class ApplicationDbContext
{
    public IMongoCollection<ArticleDb> ArticleCollection { get; private init; }

    public IMongoCollection<CategoryDb> CategoryCollection { get; private init; }

    public ApplicationDbContext(DocumentationArticleDbSettings settings)
    {
        MongoClient client = new(settings.ConnectionString);
        var dataBase = client.GetDatabase(settings.DatabaseName);

        ArticleCollection = dataBase.GetCollection<ArticleDb>(settings.ArticlesCollectionName);
        CategoryCollection = dataBase.GetCollection<CategoryDb>(settings.CategoriesCollectionName);

        ArticleCollection.SeedArticles();
        CategoryCollection.SeedCategories();
    }
}
