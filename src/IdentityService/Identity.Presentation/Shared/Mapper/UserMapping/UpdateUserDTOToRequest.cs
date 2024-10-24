using AutoMapper;
using Identity.Application.Services.Requests.UserRequests;
using Identity.Presentation.Shared.Models.DTOs.User;

namespace Identity.Presentation.Shared.Mapper.UserMapping;

public class UpdateUserDTOToRequest : Profile
{
    public UpdateUserDTOToRequest() 
    {
        CreateMap<UpdateUserRequestDTO, UpdateUserRequest>()
            .ForMember(d => d.NewUsername, opt => opt.MapFrom(src => src.NewUsername));
    }
}
