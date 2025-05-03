using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

internal sealed class PublishArticleCommandHandler : IRequestHandler<PublishArticleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public PublishArticleCommandHandler(
        IUnitOfWork unitOfWork,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUserProvider = currentUserProvider;
    }

    public async Task Handle(PublishArticleCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.ArticleId, cancellationToken);

        if (dbArticle is null)
        {
            throw new GuardNotFoundException($"Article with id {request.ArticleId} was not found");
        }

        if (dbArticle.IsPublished)
        {
            throw new GuardArgumentException($"This article has already been published");
        }

        if (dbArticle.AuthorId != currentUser.Id)
        {
            throw new GuardForbiddenException($"The user with id {currentUser.Id} is not author of this article");
        }

        dbArticle.IsPublished = true;
        dbArticle.DateOfPublication = DateTimeOffset.UtcNow;

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(dbArticle, cancellationToken);
    }
}
