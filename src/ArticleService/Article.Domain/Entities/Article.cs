using Core.Abstractions;

namespace Article.Domain.Entities;

public class Article : BaseEntity
{
    public string Title { get; set; } = null!;

    public bool IsPublished { get; set; }

    public DateTimeOffset? DateOfPublication { get; set; }

    public Guid AuthorId { get; set; }

    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    public ICollection<Block> Blocks { get; set; } = null!;
}
