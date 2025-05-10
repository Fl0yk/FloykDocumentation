using Article.Domain.Entities;
using AutoMapper;
using Core.Models.Events;

namespace Article.Infrastructure.Shared.Mappers.Users;

public class UserUpdatedEventToUser : Profile
{
    public UserUpdatedEventToUser()
    {
        CreateMap<UserUpdatedEvent, User>()
            .ForMember(d => d.PublicUsername, opt => opt.MapFrom(src => src.PublicUsername));
    }
}
