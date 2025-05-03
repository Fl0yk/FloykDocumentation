using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using MediatR;

namespace Article.Application.UseCases.ComandHandler.Articles;

internal sealed class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBaseCurrentUserProvider _currentUserProvider;
    public UpdateArticleCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IBaseCurrentUserProvider currentUserProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbArticle = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Id, cancellationToken);

        if (dbArticle is null)
        {
            throw new GuardNotFoundException($"Article with id {request.Id} was not found");
        }

        if (dbArticle.AuthorId != currentUser.Id)
        {
            throw new GuardForbiddenException($"The user with id {currentUser.Id} is not author of this article");
        }

        _mapper.Map(request, dbArticle);

        await _unitOfWork.ArticleRepository.UpdateArticleAsync(dbArticle, cancellationToken);
    }
}
