using AutoMapper;
using Identity.Application.Services.Requests.IdentityRequests;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Mapper.IdentityMapping;

public class AddUserToRoleDTOToRequest : Profile
{
    public AddUserToRoleDTOToRequest()
    {
        CreateMap<AddUserToRoleRequestDTO, AddUserToRoleRequest>()
            .ForMember(d => d.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(d => d.RoleName, opt => opt.MapFrom(src => src.RoleName));
    }
}
