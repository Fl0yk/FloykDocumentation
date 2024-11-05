using AutoMapper;
using Identity.Application.Shared.Models.Requests.UserRequests;
using Identity.Presentation.Shared.Models.DTOs.User;

namespace Identity.Presentation.Shared.Mapper.UserMapping;

public class SaveArticleDTOToRequest : Profile
{
    public SaveArticleDTOToRequest()
    {
        CreateMap<SaveArticleRequestDTO, SaveArticleRequest>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.ArticleName, opt => opt.MapFrom(src => src.ArticleName));
    }
}
