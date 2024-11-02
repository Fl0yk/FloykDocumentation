using MongoDB.Bson.Serialization.Attributes;

namespace Article.Infrastructure.Shared.Models;

public class ArticleDb
{
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;

    public bool IsPublished { get; set; }

    public DateTime? DateOfPublication { get; set; }

    public Guid CategoryId { get; set; }

    [BsonIgnore]
    public CategoryDb? Category { get; set; }

    public ICollection<BlockDb> Blocks { get; set; } = [];
}
