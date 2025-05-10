using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using Core.Exceptions;
using Core.Models.Events;
using Core.Providers.Interfaces;
using MassTransit;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

public sealed class ApproveArticleCommandHandler : IRequestHandler<ApproveArticleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ITransactionProvider _transactionProvider;

    public ApproveArticleCommandHandler(
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint,
        ITransactionProvider transactionProvider)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
        _transactionProvider = transactionProvider;
    }

    public async Task Handle(ApproveArticleCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var article = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.ArticleId, cancellationToken);

        if (article is null)
        {
            throw new GuardNotFoundException($"Article by id '{request.ArticleId}' not found");
        }

        var author = await _unitOfWork.UserRepository.GetByIdAsync(article.AuthorId, cancellationToken);

        if (author is null)
        {
            throw new GuardNotFoundException($"User with id {article.AuthorId} not found");
        }

        if (!article.IsShouldBeApproved)
        {
            throw new GuardArgumentException($"Article with id {article.Id} should not be approved");
        }

        article.IsShouldBeApproved = false;

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(article, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(new ArticleApprovedEvent()
        {
            UserId = author.Id
        }, cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
    }
}
