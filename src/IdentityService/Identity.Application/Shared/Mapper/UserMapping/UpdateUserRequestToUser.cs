using AutoMapper;
using Identity.Application.UseCases.Command.Users;
using Identity.Domain.Entities;

namespace Identity.Application.Shared.Mapper.UserMapping;

public class UpdateUserRequestToUser : Profile
{
    public UpdateUserRequestToUser()
    {
        CreateMap<UpdateUserCommand, User>()
            .ForMember(d => d.PublicUsername, opt => opt.MapFrom(src => src.NewPublicUsername));
    }
}
