namespace Article.Infrastructure;

public class DocumentationArticleDbSettings
{
    public required string CategoriesCollectionName { get; init; }

    public required string ArticlesCollectionName { get; init; }

    public required string ConnectionString { get; init; }

    public required string DatabaseName { get; init; }
}
