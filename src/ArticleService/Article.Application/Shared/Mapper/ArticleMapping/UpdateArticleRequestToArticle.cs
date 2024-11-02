using Article.Application.UseCases.Requests.Articles;
using AutoMapper;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Application.Shared.Mapper.ArticleMapping;

public class UpdateArticleRequestToArticle : Profile
{
    public UpdateArticleRequestToArticle() 
    {
        CreateMap<UpdateArticleRequest, ArticleModel>()
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.NewTitle));
    }
}
