using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

public sealed class UnsaveArticleCommandHandler : IRequestHandler<UnsaveArticleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseCurrentUserProvider _currentrUserProvider;
    private readonly ITransactionProvider _transactionProvider;

    public UnsaveArticleCommandHandler(
        IUnitOfWork unitOfWork,
        IBaseCurrentUserProvider currentrUserProvider,
        ITransactionProvider transactionProvider)
    {
        _unitOfWork = unitOfWork;
        _currentrUserProvider = currentrUserProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task Handle(UnsaveArticleCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var initiator = _currentrUserProvider.GetCurrentUser();

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

        var savedArticle = dbUser.SavedArticles.FirstOrDefault(x => x.ArticleId == request.ArticleId);

        if (savedArticle is null)
        {
            throw new GuardArgumentException($"Article with id {dbArticle.Id} not saved. User id: {dbUser.Id}");
        }

        await _unitOfWork.UserRepository.UnsaveArticle(savedArticle, cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
    }
}
