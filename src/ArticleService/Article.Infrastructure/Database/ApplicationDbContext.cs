using Article.Infrastructure.Data;
using Article.Infrastructure.Data.Seeders;
using Article.Infrastructure.Shared.Models;
using MongoDB.Driver;

namespace Article.Infrastructure.Database;

public class ApplicationDbContext
{
    public IMongoCollection<ArticleDb> ArticleCollection { get; private init; }

    public ApplicationDbContext(DocumentationArticleDbSettings settings)
    {
        MongoClient client = new(settings.ConnectionString);
        var dataBase = client.GetDatabase(settings.DatabaseName);

        ArticleCollection = dataBase.GetCollection<ArticleDb>(settings.ArticlesCollectionName);
        //TODO: индексы для статей?
        ArticleCollection.SeedArticles();
    }
}
