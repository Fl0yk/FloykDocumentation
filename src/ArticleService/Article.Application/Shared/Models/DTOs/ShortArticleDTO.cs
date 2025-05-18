namespace Article.Application.Shared.Models.DTOs;

public class ShortArticleDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public Guid CategoryId { get; set; }

    public string AuthorUsername { get; set; } = null!;

    public string AuthorPublicUsername { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public bool IsPublished { get; set; }

    public bool IsDocumentation { get; set; }

    public bool IsShouldBeApproved { get; set; }

    public DateTimeOffset? DateOfPublication { get; set; }
}
