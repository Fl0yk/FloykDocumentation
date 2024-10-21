using AutoMapper;
using Forum.Application.UseCase.Command.Answer;
using Forum.Presentation.Shared.Models.DTOs.Answer;

namespace Forum.Presentation.Shared.Mapper.AnswerMapping;

public class UpdateAnswerRequestToCommand : Profile
{
    public UpdateAnswerRequestToCommand()
    {
        CreateMap<UpdateAnswerRequestDTO, UpdateAnswerCommand>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Text));
    }
}
