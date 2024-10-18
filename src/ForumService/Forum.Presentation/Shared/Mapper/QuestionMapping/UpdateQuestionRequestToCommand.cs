using AutoMapper;
using Forum.Application.UseCase.Command.Question;
using Forum.Presentation.Shared.Models.DTOs.Question;

namespace Forum.Presentation.Shared.Mapper.QuestionMapping;

public class UpdateQuestionRequestToCommand : Profile
{
    public UpdateQuestionRequestToCommand()
    {
        CreateMap<UpdateQuestionRequestDTO, UpdateQuestionCommand>()
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description));
    }
}
