using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Requests.Categories;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using MediatR;

namespace Article.Application.UseCases.Interactors.Category;

public class GetAllCategoriesInteractor : IRequestHandler<GetAllCategoriesRequest, IEnumerable<CategoryDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCategoriesInteractor(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDTO>> Handle(GetAllCategoriesRequest request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.CatergoryRepository.GetAllCategoriesAsync(cancellationToken);

        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }
}
