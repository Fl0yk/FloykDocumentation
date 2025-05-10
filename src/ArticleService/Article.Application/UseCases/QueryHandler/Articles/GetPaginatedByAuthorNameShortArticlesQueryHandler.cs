using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using MediatR;

namespace Article.Application.UseCases.QueryHandler.Articles;

public class GetPaginatedByAuthorNameShortArticlesQueryHandler 
    : IRequestHandler<GetPaginatedByAuthorNameShortArticlesQuery, PaginatedResult<ShortArticleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaginatedByAuthorNameShortArticlesQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ShortArticleDTO>> Handle(GetPaginatedByAuthorNameShortArticlesQuery request, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.UserRepository.GetByIdAsync(request.AuthorId, cancellationToken);

        if (author is null)
        {
            throw new GuardNotFoundException($"Author with id {request.AuthorId} not found");
        }

        var articles = await _unitOfWork.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(author.Id, request.PageNo, request.PageSize, cancellationToken);

        if (!articles.Any())
        {
            throw new GuardArgumentException("Get an empty articles page");
        }

        long count = await _unitOfWork.ArticleRepository.CountByAuthorAsync(author.Id, cancellationToken);

        return new()
        {
            Items = _mapper.Map<IEnumerable<ShortArticleDTO>>(articles),
            TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
            CurrentPage = request.PageNo,
            PageSize = request.PageSize
        };
    }
}
