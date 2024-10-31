﻿using Article.Infrastructure.Shared.Models;
using AutoMapper;

using CategoryModel = Article.Domain.Entities.Category;

namespace Article.Infrastructure.Shared.Mappers.Category;

public class CategoryDbToCategory : Profile
{
    public CategoryDbToCategory()
    {
        CreateMap<CategoryDb, CategoryModel>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Level, opt => opt.MapFrom(src => src.Level))
            .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name));
    }
}
