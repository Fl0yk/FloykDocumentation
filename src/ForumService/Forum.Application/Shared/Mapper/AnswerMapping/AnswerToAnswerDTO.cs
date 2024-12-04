using AutoMapper;
using Forum.Application.Shared.Models.DTOs;
using Forum.Domain.Entities;

namespace Forum.Application.Shared.Mapper.AnswerMapping;
public class AnswerToAnswerDTO : Profile
{

    public AnswerToAnswerDTO()
    {
        CreateMap<Answer, AnswerDTO>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId))
            .ForMember(d => d.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(d => d.Level, opt => opt.MapFrom(src => src.Level))
            .ForMember(d => d.TimeOfCreation, opt => opt.MapFrom(src => src.TimeOfCreation))
            .ForMember(d => d.AuthorId, opt => opt.MapFrom(src => src.AuthorId));
    }
}
