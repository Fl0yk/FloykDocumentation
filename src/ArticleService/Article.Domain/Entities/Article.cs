namespace Article.Domain.Entities;

public class Article
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;

    public bool IsPublished { get; set; }

    public required Category Category { get; set; }

    public ICollection<Block> Blocks { get; set; } = [];
}
