using Article.Application.Shared.Models.DTOs;
using Article.Infrastructure.gRPC.Protos;
using AutoMapper;

namespace Article.Infrastructure.Shared.Mappers.Article;

public class ArticleDTOTogRPC : Profile
{
    public ArticleDTOTogRPC()
    {
        CreateMap<ArticleDTO, GetArticleByIdResponse>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title));
    }
}
