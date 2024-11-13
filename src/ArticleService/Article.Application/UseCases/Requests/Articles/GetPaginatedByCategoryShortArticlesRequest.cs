using Article.Application.Shared.Models;
using Article.Application.Shared.Models.DTOs;
using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class GetPaginatedByCategoryShortArticlesRequest(Guid CategoryId, int PageNo, int PageSize) : IRequest<PaginatedResult<ShortArticleDTO>>;
