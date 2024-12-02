using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class IsArticleExistByIdInteractor : IRequestHandler<IsArticleExistByIdRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public IsArticleExistByIdInteractor(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(IsArticleExistByIdRequest request, CancellationToken cancellationToken)
    {
        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Id, cancellationToken);

        return dbArticle is not null;
    }
}
