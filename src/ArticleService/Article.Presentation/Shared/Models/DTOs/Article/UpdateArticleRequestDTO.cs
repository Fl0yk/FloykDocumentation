namespace Article.Presentation.Shared.Models.DTOs.Article;

public sealed class UpdateArticleRequestDTO
{
    public Guid Id { get; set; }
    public string NewTitle { get; set; } = null!;  
}
