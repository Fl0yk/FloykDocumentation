using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.QueryHandler.Articles;

public sealed class GetCurrentUserShortArticlesQueryHandler : IRequestHandler<GetCurrentUserShortArticlesQuery, PaginatedResult<ShortArticleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public GetCurrentUserShortArticlesQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<PaginatedResult<ShortArticleDTO>> Handle(GetCurrentUserShortArticlesQuery request, CancellationToken cancellationToken)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbUser = await _unitOfWork.UserRepository.GetByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new GuardNotFoundException("Db user is null");
        }

        var articles = await _unitOfWork.ArticleRepository.GetPaginatedByAuthorWithoutBlocksArticlesAsync(user.Id, request.PageNo, request.PageSize, cancellationToken);

        long count = await _unitOfWork.ArticleRepository.CountByAuthorAsync(dbUser.Id, cancellationToken);

        var result = new PaginatedResult<ShortArticleDTO>()
        {
            Items = _mapper.Map<IEnumerable<ShortArticleDTO>>(articles),
            TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
            CurrentPage = request.PageNo,
            PageSize = request.PageSize
        };

        foreach (var article in result.Items)
        {
            article.AuthorUsername = dbUser.Username;
            article.AuthorPublicUsername = dbUser.PublicUsername;
        }

        return result;
    }
}
