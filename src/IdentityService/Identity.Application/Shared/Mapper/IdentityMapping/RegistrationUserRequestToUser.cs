using AutoMapper;
using Identity.Application.UseCases.Command.Identity;
using Identity.Domain.Entities;

namespace Identity.Application.Shared.Mapper.Identity;

public class RegistrationUserRequestToUser : Profile
{
    public RegistrationUserRequestToUser()
    {
        CreateMap<RegisterCommand, User>()
            .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.Username))
            .ForMember(d => d.PublicUsername, opt => opt.MapFrom(src => src.Username));
    }
}
