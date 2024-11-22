using Article.Application.Shared.Exceptions;
using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using Article.Domain.Abstractions.Services;
using AutoMapper;
using MediatR;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Application.UseCases.Interactors.Article;

public class CreateArticleInteractor : IRequestHandler<CreateArticleRequest>
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateArticleInteractor(IUserService userService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateArticleRequest request, CancellationToken cancellationToken)
    {
        bool isUserExist = await _userService.IsUserExist(request.AuthorName, cancellationToken);

        if (!isUserExist)
        {
            throw new NotFoundException($"User with username {request.AuthorName} was not found");
        }

        var dbCategory = await _unitOfWork.CatergoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);

        if (dbCategory is null)
        {
            throw new NotFoundException($"Category whit id {request.CategoryId} was not found");
        }

        var article = _mapper.Map<ArticleModel>(request);

        await _unitOfWork.ArticleRepository.CreateArticleAsync(article, cancellationToken);
    }
}
