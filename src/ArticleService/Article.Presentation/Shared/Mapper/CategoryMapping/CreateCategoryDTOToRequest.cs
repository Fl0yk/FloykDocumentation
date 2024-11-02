using Article.Application.UseCases.Requests.Categories;
using Article.Presentation.Shared.Models.DTOs.Category;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.CategoryMapping;

public class CreateCategoryDTOToRequest : Profile
{
    public CreateCategoryDTOToRequest()
    {
        CreateMap<CreateCategoryRequestDTO, CreateCategoryRequest>()
            .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name));
    }
}
