using AutoMapper;
using Identity.Application.UseCases.Command.Identity;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Mapper.IdentityMapping;

public class LoginUserDtoToCommand : Profile
{
    public LoginUserDtoToCommand()
    {
        CreateMap<LoginUserRequestDTO, LoginCommand>()
            .ForMember(d => d.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password));
    }
}
