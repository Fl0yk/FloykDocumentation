namespace Article.Application.UseCases.Query.Articles;

public sealed class GetCurrentUserShortArticlesQuery
{
    public int PageNo { get; init; }

    public int PageSize { get; init; }
}
