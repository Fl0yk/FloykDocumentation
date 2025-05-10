namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class GetPaginatedByAuthorArticlesRequestDTO(int PageNo, int PageSize, Guid AuthorId);
