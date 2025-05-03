using AutoMapper;
using Identity.Application.Shared.Models.DTOs;
using Identity.Domain.Entities;

namespace Identity.Application.Shared.Mapper.UserMapping;

public class UserToUserDTO : Profile
{
    public UserToUserDTO()
    {
        CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.Followings, opt => opt.MapFrom(src => src.Followings));

        CreateMap<Following, FollowingDTO>()
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author!.UserName))
            .ForMember(dest => dest.DateOfFollow, opt => opt.MapFrom(src => src.CreatedAt));
    }
}
