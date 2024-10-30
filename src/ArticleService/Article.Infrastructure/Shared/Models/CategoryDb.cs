namespace Article.Infrastructure.Shared.Models;

public class CategoryDb
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public CategoryDb? Parent { get; set; }

    public ICollection<Guid> ArticleIds { get; set; } = [];

    public ICollection<ArticleDb> Articles { get; set; } = [];
}
