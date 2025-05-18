using Article.Application.UseCases.Comand.Articles;
using AutoMapper;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Application.Shared.Mapper.ArticleMapping;

public class UpdateArticleRequestToCommand : Profile
{
    public UpdateArticleRequestToCommand() 
    {
        CreateMap<UpdateArticleCommand, ArticleModel>()
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.NewTitle))
            .ForMember(d => d.ShortDescription, opt => opt.MapFrom(src => src.NewShortDescription));
    }
}
