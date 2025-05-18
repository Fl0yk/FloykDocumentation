namespace Article.Presentation.Shared.Models.DTOs.Article;

public record class GetPopularPaginatedArticlesRequestDTO(int PageNo, int PageSize, IEnumerable<Guid>? Categories, bool? IsDocumentation);
