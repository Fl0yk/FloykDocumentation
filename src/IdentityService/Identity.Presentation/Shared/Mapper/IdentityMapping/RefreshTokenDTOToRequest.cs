using AutoMapper;
using Identity.Application.Shared.Models.Requests.IdentityRequests;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Mapper.IdentityMapping;

public class RefreshTokenDTOToRequest : Profile
{
    public RefreshTokenDTOToRequest()
    {
        CreateMap<RefreshTokenRequestDTO, RefreshTokenRequest>()
            .ForMember(d => d.Jwt, opt => opt.MapFrom(src => src.Jwt))
            .ForMember(d => d.Refresh, opt => opt.MapFrom(src => src.Refresh));
    }
}
