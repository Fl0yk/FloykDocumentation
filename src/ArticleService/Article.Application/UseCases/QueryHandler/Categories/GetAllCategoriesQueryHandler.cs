using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Categories;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using MediatR;

namespace Article.Application.UseCases.QueryHandler.Categories;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.CatergoryRepository.GetAllCategoriesAsync(cancellationToken);

        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }
}
