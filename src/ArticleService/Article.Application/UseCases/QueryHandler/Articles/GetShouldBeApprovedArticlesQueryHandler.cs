using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.QueryHandler.Articles;

public class GetShouldBeApprovedArticlesQueryHandler : IRequestHandler<GetShouldBeApprovedArticlesQuery, IEnumerable<ShortArticleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public GetShouldBeApprovedArticlesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<IEnumerable<ShortArticleDTO>> Handle(GetShouldBeApprovedArticlesQuery request, CancellationToken cancellationToken)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null || !user.IsAdmin())
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var articles = await _unitOfWork.ArticleRepository.GetShouldBeApprovedArticlesAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<ShortArticleDTO>>(articles);

        foreach (var article in result)
        {
            var dbUser = await _unitOfWork.UserRepository.GetByIdAsync(article.AuthorId, cancellationToken);

            if (dbUser is null)
            {
                continue;
            }

            article.AuthorUsername = dbUser.Username;
            article.AuthorPublicUsername = dbUser.PublicUsername;
        }

        return result;
    }
}
