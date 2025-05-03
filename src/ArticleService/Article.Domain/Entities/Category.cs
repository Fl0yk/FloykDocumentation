using Core.Abstractions;

namespace Article.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public Guid? ParentId { get; set; }

    public Category? Parent { get; set; }

    public IEnumerable<Category> ChildCategories { get; set; } = null!;
}
