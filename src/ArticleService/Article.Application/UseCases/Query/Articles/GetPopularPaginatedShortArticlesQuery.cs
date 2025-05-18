using Article.Application.Shared.Models.DTOs;
using Core.Models;
using MediatR;

namespace Article.Application.UseCases.Query.Articles;

public sealed class GetPopularPaginatedShortArticlesQuery : IRequest<PaginatedResult<ShortArticleDTO>>
{
    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public IEnumerable<Guid>? Categories { get; set; }

    public bool? IsDocumentation { get; set; }
}