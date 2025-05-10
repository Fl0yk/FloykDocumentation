using AutoMapper;
using Identity.Application.UseCases.Command.Identity;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Mapper.IdentityMapping;

public class AddUserToRoleDtoToCommand : Profile
{
    public AddUserToRoleDtoToCommand()
    {
        CreateMap<AddUserToRoleRequestDTO, AddUserToRoleCommand>()
            .ForMember(d => d.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(d => d.RoleName, opt => opt.MapFrom(src => src.RoleName));
    }
}
