using MongoDB.Bson.Serialization.Attributes;

namespace Article.Infrastructure.Shared.Models;

public class ArticleDb
{
    [BsonId]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public bool IsPublished { get; set; }

    public bool IsShouldBeApproved { get; set; }

    public DateTimeOffset? DateOfPublication { get; set; }

    public Guid CategoryId { get; set; }

    public ICollection<BlockDb> Blocks { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }
}
