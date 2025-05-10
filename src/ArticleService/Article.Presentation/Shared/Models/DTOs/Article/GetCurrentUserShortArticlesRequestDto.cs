namespace Article.Presentation.Shared.Models.DTOs.Article;

public sealed class GetCurrentUserShortArticlesRequestDto
{
    public int PageNo { get; init; } 
    public int PageSize { get; init; }
}
