using AutoMapper;
using Forum.Application.Answers.Commands.UpdateAnswer;
using Forum.Domain.Entities;

namespace Forum.Application.Shared.Mapper.AnswerMapping;
public class UpdateAnswerCommandToAnswer : Profile
{
    public UpdateAnswerCommandToAnswer()
    {
        CreateMap<UpdateAnswerCommand, Answer>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Text));
    }
}
