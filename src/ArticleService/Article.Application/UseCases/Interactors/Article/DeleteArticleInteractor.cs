using Article.Application.Shared.Exceptions;
using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class DeleteArticleInteractor : IRequestHandler<DeleteArticleRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteArticleInteractor(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteArticleRequest request, CancellationToken cancellationToken)
    {
        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Id, cancellationToken);

        if (dbArticle is null)
        {
            throw new NotFoundException($"Article with id {request.Id} was not found");
        }

        if (dbArticle.AuthorName != request.AuthorName)
        {
            throw new ForbiddenException($"Author {request.AuthorName} cannot delete this article");
        }


        await _unitOfWork.ArticleRepository.DeleteArticleAsync(dbArticle, cancellationToken);
    }
}
