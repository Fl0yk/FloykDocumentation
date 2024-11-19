﻿using AutoMapper;
using Identity.Application.Shared.Models.Requests.IdentityRequests;
using Identity.Presentation.Shared.Models.DTOs.Identity;

namespace Identity.Presentation.Shared.Mapper.IdentityMapping;

public class LoginUserDTOToRequest : Profile
{
    public LoginUserDTOToRequest()
    {
        CreateMap<LoginUserRequestDTO, LoginUserRequest>()
            .ForMember(d => d.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(d => d.Password, opt => opt.MapFrom(src => src.Password));
    }
}