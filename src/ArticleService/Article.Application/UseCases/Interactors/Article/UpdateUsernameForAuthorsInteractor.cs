using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Abstractions.Repositories;
using MediatR;

namespace Article.Application.UseCases.Interactors.Article;

public class UpdateUsernameForAuthorsInteractor : IRequestHandler<UpdateUsernameForAuthorsRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUsernameForAuthorsInteractor(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateUsernameForAuthorsRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ArticleRepository.UpdateAuthorsNamesAsync(request.OldUsername, request.NewUsername, cancellationToken);
    }
}
