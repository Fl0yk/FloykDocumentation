using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.QueryHandler.Articles;

public sealed class GetSavedArticlesByUserQueryHandler : IRequestHandler<GetSavedArticlesByUserQuery, IEnumerable<ShortArticleDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseCurrentUserProvider _currentUserProvider;
    private readonly IMapper _mapper;
    
    public GetSavedArticlesByUserQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUserProvider = currentUserProvider;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShortArticleDTO>> Handle(GetSavedArticlesByUserQuery request, CancellationToken cancellationToken)
    {
        var initiator = _currentUserProvider.GetCurrentUser();

        if (initiator is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbUser = await _unitOfWork.UserRepository.GetByIdAsync(initiator.Id, cancellationToken);
        dbUser = await _unitOfWork.UserRepository.WithSavedArticles(dbUser, cancellationToken);

        if (dbUser is null)
        {
            throw new GuardNotFoundException($"User with id {initiator.Id} was not found");
        }

        var articles = await _unitOfWork.ArticleRepository.GetArticlesByIdsAsync(dbUser.SavedArticles.Select(x => x.ArticleId), cancellationToken);

        return _mapper.Map<IEnumerable<ShortArticleDTO>>(articles);
    }
}
