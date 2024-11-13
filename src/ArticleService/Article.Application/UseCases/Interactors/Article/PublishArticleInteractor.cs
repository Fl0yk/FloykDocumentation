using Article.Application.Shared.Exceptions;
using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class PublishArticleInteractor : IRequestHandler<PublishArticleRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public PublishArticleInteractor(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PublishArticleRequest request, CancellationToken cancellationToken)
    {
        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.ArticleId, cancellationToken);

        if (dbArticle is null)
        {
            throw new NotFoundException($"Article with id {request.ArticleId} was not found");
        }

        if (dbArticle.IsPublished)
        {
            throw new BadRequestException($"This article has already been published");
        }

        if (dbArticle.AuthorName != request.AuthorName)
        {
            throw new ForbiddenException($"The user {request.AuthorName} is not author of this article");
        }

        dbArticle.IsPublished = true;
        dbArticle.DateOfPublication = DateTime.UtcNow;

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(dbArticle, cancellationToken);
    }
}
