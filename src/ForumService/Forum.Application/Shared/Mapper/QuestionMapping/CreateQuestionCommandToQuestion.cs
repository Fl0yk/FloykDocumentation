using AutoMapper;
using Forum.Application.UseCase.Command.Question;
using Forum.Domain.Entities;

namespace Forum.Application.Shared.Mapper.QuestionMapping;
public class CreateQuestionCommandToQuestion : Profile
{
    public CreateQuestionCommandToQuestion()
    {
        CreateMap<CreateQuestionCommand, Question>()
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(d => d.AuthorId, opt => opt.MapFrom(src => src.AuthorId));
    }
}
