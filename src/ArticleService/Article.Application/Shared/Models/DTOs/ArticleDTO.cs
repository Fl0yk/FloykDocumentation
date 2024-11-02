using Article.Domain.Entities;

namespace Article.Application.Shared.Models.DTOs;

public class ArticleDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;

    public bool IsPublished { get; set; }

    public DateTime? DateOfPublication { get; set; }

    public required Guid CategoryId { get; set; }

    public ICollection<Block> Blocks { get; set; } = [];
}
