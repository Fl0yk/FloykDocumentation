namespace Article.Domain.Entities;

public class Article
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;

    public bool IsPublished { get; set; }

    public DateTime? DateOfPublication { get; set; }

    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    public ICollection<Block> Blocks { get; set; } = [];
}
