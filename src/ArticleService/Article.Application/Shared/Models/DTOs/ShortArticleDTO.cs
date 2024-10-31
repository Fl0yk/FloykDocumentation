namespace Article.Application.Shared.Models.DTOs;

public class ShortArticleDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string AuthorName { get; set; } = string.Empty;

    public bool IsPublished { get; set; }

    public DateTime DateOfPublication { get; set; }
}
