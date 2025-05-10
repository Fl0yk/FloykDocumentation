using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

internal sealed class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseCurrentUserProvider _currentUserProvider;
    private readonly ITransactionProvider _transactionProvider;

    public DeleteArticleCommandHandler(
        IUnitOfWork unitOfWork, 
        IBaseCurrentUserProvider currentUserProvider,
        ITransactionProvider transactionProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUserProvider = currentUserProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Id, cancellationToken);

        if (dbArticle is null)
        {
            throw new GuardNotFoundException($"Article with id {request.Id} was not found");
        }

        if (dbArticle.AuthorId != currentUser.Id)
        {
            throw new GuardForbiddenException($"Author with id {currentUser.Id} cannot delete this article");
        }

        await _unitOfWork.UserRepository.RemoveSavedArticlesByArticleAsync(dbArticle.Id, cancellationToken);

        await _unitOfWork.ArticleRepository.DeleteArticleAsync(dbArticle, cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
    }
}
