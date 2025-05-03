using AutoMapper;
using Forum.Application.UseCase.Command.Answer;
using Forum.Domain.Entities;

namespace Forum.Application.Shared.Mapper.AnswerMapping;
public class CreateAnswerCommandToAnswer : Profile
{
    public CreateAnswerCommandToAnswer()
    {
        CreateMap<AddAnswerCommand, Answer>()
            .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId))
            .ForMember(d => d.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Text));
    }
}
