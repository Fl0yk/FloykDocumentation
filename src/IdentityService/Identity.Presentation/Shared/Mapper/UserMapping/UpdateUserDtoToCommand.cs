using AutoMapper;
using Identity.Application.UseCases.Command.Users;
using Identity.Presentation.Shared.Models.DTOs.User;

namespace Identity.Presentation.Shared.Mapper.UserMapping;

public class UpdateUserDtoToCommand : Profile
{
    public UpdateUserDtoToCommand() 
    {
        CreateMap<UpdateUserRequestDTO, UpdateUserCommand>()
            .ForMember(d => d.NewUsername, opt => opt.MapFrom(src => src.NewUsername));
    }
}
