using Article.Application.Shared.Exceptions;
using Article.Application.UseCases.Requests.Categories;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using MediatR;

using CategoryModel = Article.Domain.Entities.Category;

namespace Article.Application.UseCases.Interactors.Category;

public class CreateCategoryInteractor : IRequestHandler<CreateCategoryRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoryInteractor(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        CategoryModel? parent = null;

        if (request.ParentId is not null)
        {
            parent = await _unitOfWork.CatergoryRepository.GetCategoryByIdAsync(request.ParentId.Value, cancellationToken);

            if (parent is null)
            {
                throw new NotFoundException($"Parent category with id {request.ParentId} was not found");
            }
        }

        var category = _mapper.Map<CategoryModel>(request);

        if (parent is not null)
        {
            category.Level = parent.Level + 1;
        }

        await _unitOfWork.CatergoryRepository.AddCategoryAsync(category, cancellationToken);
    }
}
