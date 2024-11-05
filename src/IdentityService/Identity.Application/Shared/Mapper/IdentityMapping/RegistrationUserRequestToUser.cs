using AutoMapper;
using Identity.Application.Shared.Models.Requests.IdentityRequests;
using Identity.DataAccess.Entities;

namespace Identity.Application.Shared.Mapper.Identity;

public class RegistrationUserRequestToUser : Profile
{
    public RegistrationUserRequestToUser()
    {
        CreateMap<RegistrationUserRequest, User>()
            .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.Username));
    }
}
