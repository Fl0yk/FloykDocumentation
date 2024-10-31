using Article.Domain.Entities;

namespace Article.Application.Shared.Models.DTOs;

public class CategoryDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public int Level { get; set; }
}
