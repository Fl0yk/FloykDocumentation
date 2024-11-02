using Article.Application.Shared.Exceptions;
using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Requests.Categories;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using MediatR;

namespace Article.Application.UseCases.Interactors.Category;

public class GetCategoryByIdWithoutArticlesInteractor
    : IRequestHandler<GetCategoryByIdWithoutArticlesRequest, CategoryDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdWithoutArticlesInteractor(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDTO> Handle(GetCategoryByIdWithoutArticlesRequest request, CancellationToken cancellationToken)
    {
        var dbCategory = await  _unitOfWork.CatergoryRepository.GetCategoryByIdAsync(request.Id, cancellationToken);

        if (dbCategory is null)
        {
            throw new NotFoundException($"Category with id {request.Id} was not found");
        }

        return _mapper.Map<CategoryDTO>(dbCategory);
    }
}
