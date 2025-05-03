using AutoMapper;
using Identity.Application.UseCases.Command.Identity;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Mapper.IdentityMapping;

public class RegistrationUserDtoToCommand : Profile
{
    public RegistrationUserDtoToCommand()
    {
        CreateMap<RegistrationUserRequestDTO, RegisterCommand>()
            .ForMember(d => d.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password));
    }
}
