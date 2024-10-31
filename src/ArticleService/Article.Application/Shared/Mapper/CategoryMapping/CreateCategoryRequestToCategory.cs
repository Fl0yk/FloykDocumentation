using Article.Application.UseCases.Requests.Categories;
using Article.Domain.Entities;
using AutoMapper;

namespace Article.Application.Shared.Mapper.CategoryMapping;

public class CreateCategoryRequestToCategory : Profile
{
    public CreateCategoryRequestToCategory()
    {
        CreateMap<CreateCategoryRequest, Category>()
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId));
    }
}
