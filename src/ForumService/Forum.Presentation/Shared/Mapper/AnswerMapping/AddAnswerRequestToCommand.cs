using AutoMapper;
using Forum.Application.UseCase.Command.Answer;
using Forum.Presentation.Shared.Models.DTOs.Answer;

namespace Forum.Presentation.Shared.Mapper.AnswerMapping;

public class AddAnswerRequestToCommand : Profile
{
    public AddAnswerRequestToCommand()
    {
        CreateMap<AddAnswerRequestDTO, AddAnswerCommand>()
            .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.ParentId))
            .ForMember(d => d.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
            .ForMember(d => d.AuthorId, opt => opt.MapFrom(src => src.CurrentUserId))
            .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Text));
    }
}
