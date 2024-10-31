using Article.Application.Shared.Models;
using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Requests.Articles;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class GetPaginatedByDateShortArticlesInteractor
    : IRequestHandler<GetPaginatedByDateShortArticlesRequest, PaginatedResult<ShortArticleDTO>>
{
    public Task<PaginatedResult<ShortArticleDTO>> Handle(GetPaginatedByDateShortArticlesRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
