using Article.Application.Shared.Models.DTOs;
using AutoMapper;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Application.Shared.Mapper.ArticleMapping;

public class ArticleToShortDTO : Profile
{
    public ArticleToShortDTO()
    {
        CreateMap<ArticleModel,  ShortArticleDTO>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
            .ForMember(d => d.DateOfPublication, opt => opt.MapFrom(src => src.DateOfPublication))
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title));
    }
}
