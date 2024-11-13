using Article.Infrastructure.Shared.Models;
using AutoMapper;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Infrastructure.Shared.Mappers.Article;

public class ArticleToArticleDb : Profile
{
    public ArticleToArticleDb()
    {
        CreateMap<ArticleModel, ArticleDb>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(d => d.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
            .ForMember(d => d.DateOfPublication, opt => opt.MapFrom(src => src.DateOfPublication))
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(d => d.Blocks, opt => opt.MapFrom(src => src.Blocks));
    }
}
