using MongoDB.Bson.Serialization.Attributes;

namespace Article.Infrastructure.Shared.Models;

public class BlockDb
{
    [BsonId]
    public Guid Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public required string Type { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }
}
