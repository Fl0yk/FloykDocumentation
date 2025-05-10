using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

public sealed class ApproveArticleCommandHandler : IRequestHandler<ApproveArticleCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveArticleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ApproveArticleCommand request, CancellationToken cancellationToken)
    {
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

        article.IsPublished = true;
        
        //TODO: Send event to identity

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(article, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
