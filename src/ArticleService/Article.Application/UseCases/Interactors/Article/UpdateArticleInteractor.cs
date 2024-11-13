using Article.Application.Shared.Exceptions;
using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class UpdateArticleInteractor : IRequestHandler<UpdateArticleRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateArticleInteractor(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateArticleRequest request, CancellationToken cancellationToken)
    {
        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Id, cancellationToken);

        if (dbArticle is null)
        {
            throw new NotFoundException($"Article with id {request.Id} was not found");
        }

        if (dbArticle.AuthorName != request.AuthorName)
        {
            throw new ForbiddenException($"The user {request.AuthorName} is not author of this article");
        }

        _mapper.Map(request, dbArticle);

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(dbArticle, cancellationToken);
    }
}
