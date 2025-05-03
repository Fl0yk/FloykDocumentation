using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

internal sealed class AppendBlockCommandHandler : IRequestHandler<AppendBlockCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public AppendBlockCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task Handle(AppendBlockCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var article = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.ArticleId, cancellationToken);

        if (article is null)
        {
            throw new GuardNotFoundException($"Article with id {request.ArticleId} was not found");
        }

        var author = await _unitOfWork.UserRepository.GetByIdAsync(article.AuthorId, cancellationToken);

        if (author is null || author.Id != currentUser.Id)
        {
            throw new GuardForbiddenException($"The user {currentUser.Id} is not author of this article");
        }

        if (!BlockType.Types.Contains(request.BlockType))
        {
            throw new GuardArgumentException($"Block type \"{request.BlockType}\" does not exist");
        }

        var block = _mapper.Map<Block>(request);

        article.Blocks.Add(block);

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(article, cancellationToken);
    }
}
