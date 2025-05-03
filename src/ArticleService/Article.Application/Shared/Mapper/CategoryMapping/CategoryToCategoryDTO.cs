using Article.Application.Shared.Models.DTOs;
using Article.Domain.Entities;
using AutoMapper;

namespace Article.Application.Shared.Mapper.CategoryMapping;

public sealed class CategoryToCategoryDTO : Profile
{
    public CategoryToCategoryDTO()
    {
        CreateMap<Category, CategoryDTO>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(d => d.Order, opt => opt.MapFrom(src => src.Order))
            .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId));
    }
}
