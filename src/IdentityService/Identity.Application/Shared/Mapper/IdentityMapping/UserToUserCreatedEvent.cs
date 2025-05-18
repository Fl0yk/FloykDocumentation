using AutoMapper;
using Core.Models.Events;
using Identity.Domain.Entities;

namespace Identity.Application.Shared.Mapper.IdentityMapping;

public sealed class UserToUserCreatedEvent : Profile
{
    public UserToUserCreatedEvent()
    {
        CreateMap<User, UserCreatedEvent>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Username, opt => opt.MapFrom(src => src.UserName))
            .ForMember(d => d.NormalizedUsername, opt => opt.MapFrom(src => src.NormalizedUserName))
            .ForMember(d => d.PublicUsername, opt => opt.MapFrom(src => src.UserName));
    }
}
