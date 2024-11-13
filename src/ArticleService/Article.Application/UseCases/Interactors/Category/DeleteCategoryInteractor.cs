using Article.Application.Shared.Exceptions;
using Article.Application.UseCases.Requests.Categories;
using Article.Domain.Abstractions.Repositories;
using MediatR;

namespace Article.Application.UseCases.Interactors.Category;

public class DeleteCategoryInteractor : IRequestHandler<DeleteCategoryRequest>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryInteractor(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var dbCategory = await _unitOfWork.CatergoryRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);

        if (dbCategory is null)
        {
            throw new NotFoundException($"Category with id {request.CategoryId} was not found");
        }

        if (await _unitOfWork.CatergoryRepository.IsExistArticleInCategoryAsync(request.CategoryId, cancellationToken))
        {
            throw new BadRequestException("This category already has articles");
        }

        await _unitOfWork.CatergoryRepository.DelteCategoryAsync(dbCategory.Id, cancellationToken);
    }
}
