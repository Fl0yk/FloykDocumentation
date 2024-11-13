namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class GetPaginatedByCategoryArticlesRequestDTO(Guid CategoryId, int PageNo, int PageSize);
