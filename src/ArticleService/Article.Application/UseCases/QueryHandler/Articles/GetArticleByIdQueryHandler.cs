using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.QueryHandler.Articles;

public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBaseCurrentUserProvider _currentUserProvider;

    public GetArticleByIdQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _currentUserProvider = currentUserProvider;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ArticleDTO> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Id, cancellationToken);

        if (dbArticle is null)
        {
            throw new GuardNotFoundException($"Article with id {request.Id} was not fount");
        }

        var user = _currentUserProvider.GetCurrentUser();

        if(user is not null && user.Id != dbArticle.AuthorId)
        {
            dbArticle.VisitCount++;

            await _unitOfWork.ArticleRepository.UpdateArticleAsync(dbArticle, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return _mapper.Map<ArticleDTO>(dbArticle);
    }
}
