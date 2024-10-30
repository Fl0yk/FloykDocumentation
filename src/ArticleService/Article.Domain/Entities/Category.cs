namespace Article.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Category? Parent { get; set; }

    public ICollection<Article> Articles { get; set; } = [];
}
