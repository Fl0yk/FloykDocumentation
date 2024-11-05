namespace Identity.Application.Shared.Models.DTOs;

public class SavedArticleDTO
{
    public Guid UserId { get; set; }

    public Guid ArticleId { get; set; }

    public string ArticleName { get; set; } = string.Empty;
}
