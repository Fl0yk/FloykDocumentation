using MongoDB.Bson.Serialization.Attributes;

namespace Article.Infrastructure.Shared.Models;

public class CategoryDb
{
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    [BsonIgnore]
    public CategoryDb? Parent { get; set; }

    public ICollection<Guid> ArticleIds { get; set; } = [];

    [BsonIgnore]
    public ICollection<ArticleDb> Articles { get; set; } = [];
}
