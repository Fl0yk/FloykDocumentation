namespace Article.Infrastructure.Data;

public class DocumentationArticleDbSettings
{
    public required string ArticlesCollectionName { get; init; }

    public required string ConnectionString { get; init; }

    public required string DatabaseName { get; init; }

    //TODO: add in configuration
    public required string SqlConnectionString { get; init; }
}
