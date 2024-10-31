using MongoDB.Bson.Serialization.Attributes;

namespace Article.Infrastructure.Shared.Models;

public class BlockDb
{
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Text { get; set; } = string.Empty;

    public required string Type { get; set; }
}
