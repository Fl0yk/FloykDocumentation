using Article.Application.Shared.Exceptions;
using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class GetArticleByIdInteractor : IRequestHandler<GetArticleByIdRequest, ArticleDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetArticleByIdInteractor(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ArticleDTO> Handle(GetArticleByIdRequest request, CancellationToken cancellationToken)
    {
        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Id, cancellationToken);

        if (dbArticle is null)
        {
            throw new NotFoundException($"Article with id {request.Id} was not fount");
        }

        return _mapper.Map<ArticleDTO>(dbArticle);
    }
}
