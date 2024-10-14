using AutoMapper;
using Forum.Application.Shared.Models.DTOs;
using Forum.Domain.Entities;

namespace Forum.Application.Shared.Mapper.QuestionMapping;
public class QuestionToQuestionDTO : Profile
{
    public QuestionToQuestionDTO()
    {
        CreateMap<Question, QuestionDTO>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(d => d.DateOfCreation, opt => opt.MapFrom(src => src.DateOfCreation))
            .ForMember(d => d.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(d => d.IsClosed, opt => opt.MapFrom(src => src.IsClosed));
    }
}
