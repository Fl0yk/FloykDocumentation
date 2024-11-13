using Article.Application.Shared.Exceptions;
using Article.Application.Shared.Models;
using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class GetPaginatedByDateShortArticlesInteractor
    : IRequestHandler<GetPaginatedByDateShortArticlesRequest, PaginatedResult<ShortArticleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaginatedByDateShortArticlesInteractor(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ShortArticleDTO>> Handle(GetPaginatedByDateShortArticlesRequest request, CancellationToken cancellationToken)
    {
        var articles = await _unitOfWork.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(request.PageNo, request.PageSize, cancellationToken);

        if (!articles.Any())
        {
            throw new BadRequestException("Get an empty articles page");
        }

        long count = await _unitOfWork.ArticleRepository.GetCountAsync(cancellationToken);

        return new()
        {
            Items = _mapper.Map<IEnumerable<ShortArticleDTO>>(articles),
            TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
            CurrentPage = request.PageNo,
            PageSize = request.PageSize
        };
    }
}
