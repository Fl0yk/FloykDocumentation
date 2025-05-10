using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

public sealed class SaveArticleCommandHandler : IRequestHandler<SaveArticleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseCurrentUserProvider _currentUserProvider;
    private readonly ITransactionProvider _transactionProvider;

    public SaveArticleCommandHandler(
        IUnitOfWork unitOfWork, 
        IBaseCurrentUserProvider currentrUserProvider,
        ITransactionProvider transactionProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUserProvider = currentrUserProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task Handle(SaveArticleCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var initiator = _currentUserProvider.GetCurrentUser();

        if (initiator is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.ArticleId, cancellationToken);

        if (dbArticle is null)
        {
            throw new GuardNotFoundException($"Article with id {request.ArticleId} was not found");
        }

        var dbUser = await _unitOfWork.UserRepository.GetByIdAsync(initiator.Id, cancellationToken);
        dbUser = await _unitOfWork.UserRepository.WithSavedArticles(dbUser, cancellationToken);

        if (dbUser is null)
        {
            throw new GuardNotFoundException($"User with id {initiator.Id} was not found");
        }

        if (dbUser.SavedArticles.Any(x => x.ArticleId == dbArticle.Id))
        {
            throw new GuardArgumentException($"Article with id {dbArticle.Id} already saved. User id: {dbUser.Id}");
        }

        var savedArtilce = new SavedArticle()
        {
            UserId = dbUser.Id,
            ArticleId = dbArticle.Id
        };

        await _unitOfWork.UserRepository.SaveArticle(savedArtilce, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
    }
}
