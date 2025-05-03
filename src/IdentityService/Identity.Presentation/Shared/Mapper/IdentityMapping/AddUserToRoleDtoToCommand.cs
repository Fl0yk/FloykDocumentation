using AutoMapper;
using Identity.Application.UseCases.Command.Identity;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Mapper.IdentityMapping;

public class AddUserToRoleDtoToCommand : Profile
{
    public AddUserToRoleDtoToCommand()
    {
        CreateMap<AddUserToRoleRequestDTO, AddUserToRoleCommand>()
            .ForMember(d => d.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(d => d.RoleName, opt => opt.MapFrom(src => src.RoleName));
    }
}
