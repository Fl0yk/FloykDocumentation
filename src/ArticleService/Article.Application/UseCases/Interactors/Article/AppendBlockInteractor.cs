using Article.Application.Shared.Exceptions;
using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class AppendBlockInteractor : IRequestHandler<AppendBlockRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppendBlockInteractor(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(AppendBlockRequest request, CancellationToken cancellationToken)
    {
        var article = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.ArticleId, cancellationToken);

        if (article is null)
        {
            throw new NotFoundException($"Article with id {request.ArticleId} was not found");
        }

        if (article.AuthorName != request.AuthorName)
        {
            throw new ForbiddenException($"The user {request.AuthorName} is not author of this article");
        }

        if (!BlockType.Types.Contains(request.BlockType))
        {
            throw new BadRequestException($"Block type \"{request.BlockType}\" does not exist");
        }

        var block = _mapper.Map<Block>(request);

        article.Blocks.Add(block);

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(article, cancellationToken);
    }
}
