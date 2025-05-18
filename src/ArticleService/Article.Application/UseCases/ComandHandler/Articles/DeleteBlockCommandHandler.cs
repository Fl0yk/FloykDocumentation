using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

public sealed class DeleteBlockCommandHandler : IRequestHandler<DeleteBlockCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public DeleteBlockCommandHandler(
        IUnitOfWork unitOfWork,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUserProvider = currentUserProvider;
    }

    public async Task Handle(DeleteBlockCommand request, CancellationToken cancellationToken)
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

        if (dbArticle.AuthorId != currentUser.Id)
        {
            throw new GuardForbiddenException($"Author with id {currentUser.Id} cannot delete block from this article");
        }

        var dbBlock = dbArticle.Blocks.FirstOrDefault(b => b.Id == request.BlockId);

        if (dbBlock is null)
        {
            throw new GuardNotFoundException($"Article \"{dbArticle.Title}\" does not have a block with id {request.BlockId}");
        }

        dbArticle.Blocks.Remove(dbBlock);

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(dbArticle, cancellationToken);
    }
}
