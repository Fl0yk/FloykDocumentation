using AutoMapper;
using Identity.Application.Services.Requests.IdentityRequests;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Mapper.IdentityMapping;

public class RegistrationUserDTOToRequest : Profile
{
    public RegistrationUserDTOToRequest()
    {
        CreateMap<RegistrationUserRequestDTO, RegistrationUserRequest>()
            .ForMember(d => d.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password));
    }
}
