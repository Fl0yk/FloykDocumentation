using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Application.UseCases.ComandHandler.Articles;

public sealed class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public CreateArticleCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<Guid> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var author = await _unitOfWork.UserRepository.GetByIdAsync(currentUser.Id, cancellationToken);

        if (author is null)
        {
            throw new GuardNotFoundException($"User with id {currentUser.Id} was not found");
        }

        var dbCategory = await _unitOfWork.CatergoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);

        if (dbCategory is null)
        {
            throw new GuardNotFoundException($"Category whit id {request.CategoryId} was not found");
        }

        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Id, cancellationToken);
        
        if (dbArticle is not null)
        {
            throw new GuardArgumentException("Article id alreafy exist");
        }

        var article = _mapper.Map<ArticleModel>(request);
        article.AuthorId = author.Id;

        await _unitOfWork.ArticleRepository.CreateArticleAsync(article, cancellationToken);

        return article.Id;
    }
}
