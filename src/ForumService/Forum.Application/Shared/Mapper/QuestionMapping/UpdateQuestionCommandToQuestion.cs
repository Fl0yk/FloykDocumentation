using AutoMapper;
using Forum.Application.Questions.Commands.UpdateQuestion;
using Forum.Domain.Entities;

namespace Forum.Application.Shared.Mapper.QuestionMapping;
public class UpdateQuestionCommandToQuestion : Profile
{
    public UpdateQuestionCommandToQuestion()
    {
        CreateMap<UpdateQuestionCommand, Question>()
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description));
    }
}
