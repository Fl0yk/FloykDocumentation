using Article.Domain.Entities;

namespace Article.Application.Shared.Models.DTOs;

public class ArticleDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public bool IsPublished { get; set; }

    public bool IsDocumentation { get; set; }

    public string ShortDescription { get; set; } = null!;

    public bool IsShouldBeApproved { get; set; }

    public DateTimeOffset? DateOfPublication { get; set; }

    public required Guid CategoryId { get; set; }

    public ICollection<Block> Blocks { get; set; } = null!;
}
