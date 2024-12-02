using Article.Application.Shared.Exceptions;
using Article.Application.UseCases.Requests.Articles;
using Article.Contracts.Events.Article;
using Article.Domain.Abstractions.Repositories;
using MassTransit;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class DeleteArticleInteractor : IRequestHandler<DeleteArticleRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteArticleInteractor(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
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

        await _publishEndpoint.Publish<ArticleDeleted>(new
        {
            Id = request.Id
        }, cancellationToken);
    }
}
