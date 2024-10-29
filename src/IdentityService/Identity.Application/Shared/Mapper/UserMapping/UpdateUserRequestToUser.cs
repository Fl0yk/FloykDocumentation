using AutoMapper;
using Identity.Application.Shared.Models.Requests.UserRequests;
using Identity.DataAccess.Entities;

namespace Identity.Application.Shared.Mapper.UserMapping;

public class UpdateUserRequestToUser : Profile
{
    public UpdateUserRequestToUser()
    {
        CreateMap<UpdateUserRequest, User>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.NewUsername))
            .ForMember(d => d.NormalizedUserName, opt => opt.MapFrom(src => src.NewUsername.ToUpper()));
    }
}
