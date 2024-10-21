using AutoMapper;
using Forum.Application.UseCase.Command.Question;
using Forum.Presentation.Shared.Models.DTOs.Question;

namespace Forum.Presentation.Shared.Mapper.QuestionMapping;

public class CreateQuestionRequestToCommand : Profile
{
    public CreateQuestionRequestToCommand()
    {
        CreateMap<CreateQuestionRequestDTO, CreateQuestionCommand>()
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(d => d.AuthorId, opt => opt.MapFrom(src => src.AuthorId));
    }
}
