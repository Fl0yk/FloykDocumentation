using AutoMapper;
using Identity.Application.UseCases.Command.Users;
using Identity.Domain.Entities;

namespace Identity.Application.Shared.Mapper.UserMapping;

public class UpdateUserRequestToUser : Profile
{
    public UpdateUserRequestToUser()
    {
        CreateMap<UpdateUserCommand, User>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.NewUsername))
            .ForMember(d => d.NormalizedUserName, opt => opt.MapFrom(src => src.NewUsername.ToUpper()));
    }
}
