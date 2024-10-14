using AutoMapper;
using Forum.Application.Shared.Models.DTOs;
using Forum.Domain.Entities;
using Forum.Domain.Models;

namespace Forum.Application.Shared.Mapper.QuestionMapping;
public class PaginatedQuestionToPaginatedQuestionDTO : Profile
{
    public PaginatedQuestionToPaginatedQuestionDTO()
    {
        CreateMap<PaginatedResult<Question>, PaginatedResult<QuestionDTO>>()
            .ForMember(d => d.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(d => d.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(d => d.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(d => d.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
    }
}
