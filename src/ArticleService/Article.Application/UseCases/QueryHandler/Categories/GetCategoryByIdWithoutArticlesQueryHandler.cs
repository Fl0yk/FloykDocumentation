using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Categories;
using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using MediatR;

namespace Article.Application.UseCases.QueryHandler.Categories;

public class GetCategoryByIdWithoutArticlesQueryHandler
    : IRequestHandler<GetCategoryByIdWithoutArticlesQuery, CategoryDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdWithoutArticlesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDTO> Handle(GetCategoryByIdWithoutArticlesQuery request, CancellationToken cancellationToken)
    {
        var dbCategory = await  _unitOfWork.CatergoryRepository.GetCategoryByIdAsync(request.Id, cancellationToken);

        if (dbCategory is null)
        {
            throw new GuardNotFoundException($"Category with id {request.Id} was not found");
        }

        return _mapper.Map<CategoryDTO>(dbCategory);
    }
}
