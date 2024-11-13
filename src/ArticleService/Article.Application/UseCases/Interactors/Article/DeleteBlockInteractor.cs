using Article.Application.Shared.Exceptions;
using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class DeleteBlockInteractor : IRequestHandler<DeleteBlockRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBlockInteractor(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteBlockRequest request, CancellationToken cancellationToken)
    {
        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.ArticleId, cancellationToken);

        if (dbArticle is null)
        {
            throw new NotFoundException($"Article with id {request.ArticleId} was not found");
        }

        if (dbArticle.AuthorName != request.AuthorName)
        {
            throw new ForbiddenException($"Author {request.AuthorName} cannot delete block from this article");
        }

        var dbBlock = dbArticle.Blocks.FirstOrDefault(b => b.Id == request.BlockId);

        if (dbBlock is null)
        {
            throw new NotFoundException($"Article \"{dbArticle.Title}\" does not have a block with id {request.BlockId}");
        }

        dbArticle.Blocks.Remove(dbBlock);

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(dbArticle, cancellationToken);
    }
}
