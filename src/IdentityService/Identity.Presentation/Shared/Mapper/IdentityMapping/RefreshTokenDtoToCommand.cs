using AutoMapper;
using Identity.Application.UseCases.Command.Identity;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Mapper.IdentityMapping;

public class RefreshTokenDtoToCommand : Profile
{
    public RefreshTokenDtoToCommand()
    {
        CreateMap<RefreshTokenRequestDTO, RefreshTokenCommand>()
            .ForMember(d => d.JwtToken, opt => opt.MapFrom(src => src.Jwt))
            .ForMember(d => d.RefreshToken, opt => opt.MapFrom(src => src.Refresh));
    }
}
