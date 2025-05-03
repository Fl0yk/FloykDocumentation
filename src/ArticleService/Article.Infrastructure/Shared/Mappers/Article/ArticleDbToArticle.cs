using Article.Infrastructure.Shared.Models;
using AutoMapper;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Infrastructure.Shared.Mappers.Article;

public class ArticleDbToArticle : Profile
{
    public ArticleDbToArticle()
    {
        CreateMap<ArticleDb, ArticleModel>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(d => d.IsPublished, opt => opt.MapFrom(src => src.IsPublished))
            .ForMember(d => d.DateOfPublication, opt => opt.MapFrom(src => src.DateOfPublication))
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(d => d.Blocks, opt => opt.MapFrom(src => src.Blocks))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(d => d.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
            .ForMember(d => d.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));
    }
}
