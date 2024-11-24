using AutoMapper;
using Identity.DataAccess.Entities;

namespace Identity.Application.Shared.Mapper.UserMapping;

public class ArticleToSavedArticle : Profile
{
    public ArticleToSavedArticle()
    {
        CreateMap<Article, SavedArticle>()
            .ForMember(d => d.ArticleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.ArticleName, opt => opt.MapFrom(src => src.Title));
    }
}
