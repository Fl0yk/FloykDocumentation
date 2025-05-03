using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MassTransit;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

internal sealed class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public DeleteArticleCommandHandler(
        IUnitOfWork unitOfWork, 
        IPublishEndpoint publishEndpoint,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
        _currentUserProvider = currentUserProvider;
    }

    public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
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

        await _unitOfWork.ArticleRepository.DeleteArticleAsync(dbArticle, cancellationToken);

        //TODO: нужны ивенты?
        //await _publishEndpoint.Publish<ArticleDeleted>(new
        //{
        //    request.Id
        //}, cancellationToken);
    }
}
