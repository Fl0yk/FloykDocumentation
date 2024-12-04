using AutoMapper;
using Forum.Application.UseCase.Command.Answer;
using Forum.Presentation.Shared.Models.DTOs.Answer;

namespace Forum.Presentation.Shared.Mapper.AnswerMapping;

public class DeleteAnswerRequestToCommand : Profile
{
    public DeleteAnswerRequestToCommand()
    {
        CreateMap<DeleteAnswerRequestDTO, DeleteAnswerCommand>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.AnswerId))
            .ForMember(d => d.AuthorId, opt => opt.MapFrom(src => src.CurrentUserId));
    }
}
