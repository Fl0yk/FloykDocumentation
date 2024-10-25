using AutoMapper;
using Identity.Application.Services.Requests.UserRequests;
using Identity.DataAccess.Entities;

namespace Identity.Application.Shared.Mapper.UserMapping;

public class ArticleRequestToArticle : Profile
{
    public ArticleRequestToArticle()
    {
        CreateMap<SaveArticleRequest, SavedArticle>()
            .ForMember(d => d.ArticleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.ArticleName, opt => opt.MapFrom(src => src.ArticleName));
    }
}
