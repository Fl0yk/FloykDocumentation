using Article.Domain.Entities;
using AutoMapper;
using Core.Models.Events;

namespace Article.Infrastructure.Shared.Mappers.Users;

public class UserCreatedEventToUser : Profile
{
    public UserCreatedEventToUser()
    {
        CreateMap<UserCreatedEvent, User>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(d => d.NormalizedUsername, opt => opt.MapFrom(src => src.NormalizedUsername))
            .ForMember(d => d.PublicUsername, opt => opt.MapFrom(src => src.PublicUsername));
    }
}
