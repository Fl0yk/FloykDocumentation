using Article.Application.Shared.Exceptions;
using Article.Application.Shared.Models;
using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class GetPaginatedByCategoryShortArticlesInteractor
    : IRequestHandler<GetPaginatedByCategoryShortArticlesRequest, PaginatedResult<ShortArticleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaginatedByCategoryShortArticlesInteractor(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ShortArticleDTO>> Handle(GetPaginatedByCategoryShortArticlesRequest request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CatergoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);

        if (category is  null)
        {
            throw new NotFoundException($"Category with id {request.CategoryId} was not found");
        }

        var articles = await _unitOfWork.ArticleRepository.GetPaginatedByCategoryWithoutBlocksArticlesAsync(request.CategoryId, request.PageNo, request.PageSize, cancellationToken);

        if (!articles.Any())
        {
            throw new BadRequestException("Get an empty articles page");
        }

        long count = await _unitOfWork.ArticleRepository.GetCountAsync(request.CategoryId, cancellationToken);

        return new()
        {
            Items = _mapper.Map<IEnumerable<ShortArticleDTO>>(articles),
            TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
            CurrentPage = request.PageNo,
            PageSize = request.PageSize
        };
    }
}
