namespace Article.Infrastructure.Shared.Models;

public class ArticleDb
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;

    public bool IsPublished { get; set; }

    public Guid CategoryId { get; set; }

    public CategoryDb? Category { get; set; }

    public ICollection<Guid> BlockIds { get; set; } = [];

    public ICollection<BlockDb> Blocks { get; set; } = [];
}
