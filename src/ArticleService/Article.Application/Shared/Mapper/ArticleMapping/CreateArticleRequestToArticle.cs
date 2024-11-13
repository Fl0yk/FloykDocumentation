using Article.Application.UseCases.Requests.Articles;
using AutoMapper;

namespace Article.Application.Shared.Mapper.ArticleMapping;

public class CreateArticleRequestToArticle : Profile
{
    public CreateArticleRequestToArticle() 
    {
        CreateMap<CreateArticleRequest, Article.Domain.Entities.Article>()
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
    }
}
