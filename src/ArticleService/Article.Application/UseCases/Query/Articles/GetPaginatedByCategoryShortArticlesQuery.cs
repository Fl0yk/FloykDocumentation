using Article.Application.Shared.Models.DTOs;
using Core.Models;
using MediatR;

namespace Article.Application.UseCases.Query.Articles;

public sealed class GetPaginatedByCategoryShortArticlesQuery : IRequest<PaginatedResult<ShortArticleDTO>>
{
    public Guid CategoryId { get; init; }

    public int PageNo { get; init; }

    public int PageSize { get; init; }
}