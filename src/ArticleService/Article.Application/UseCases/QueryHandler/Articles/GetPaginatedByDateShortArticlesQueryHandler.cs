using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using MediatR;

namespace Article.Application.UseCases.QueryHandler.Articles;

public class GetPaginatedByDateShortArticlesQueryHandler
    : IRequestHandler<GetPaginatedByDateShortArticlesQuery, PaginatedResult<ShortArticleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaginatedByDateShortArticlesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ShortArticleDTO>> Handle(GetPaginatedByDateShortArticlesQuery request, CancellationToken cancellationToken)
    {
        var articles = await _unitOfWork.ArticleRepository.GetPaginatedByDateWithoutBlocksArticlesAsync(request.PageNo, request.PageSize, cancellationToken);

        if (!articles.Any())
        {
            throw new GuardArgumentException("Get an empty articles page");
        }

        long count = await _unitOfWork.ArticleRepository.CountAsync(cancellationToken);

        return new()
        {
            Items = _mapper.Map<IEnumerable<ShortArticleDTO>>(articles),
            TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
            CurrentPage = request.PageNo,
            PageSize = request.PageSize
        };
    }
}
