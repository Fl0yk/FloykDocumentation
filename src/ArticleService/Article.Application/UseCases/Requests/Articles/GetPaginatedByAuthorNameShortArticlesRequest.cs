using Article.Application.Shared.Models;
using Article.Application.Shared.Models.DTOs;
using MediatR;

namespace Article.Application.UseCases.Requests.Articles;

public record class GetPaginatedByAuthorNameShortArticlesRequest(int PageNo, int PageSize, string AuthorName) : IRequest<PaginatedResult<ShortArticleDTO>>;