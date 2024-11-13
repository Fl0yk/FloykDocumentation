using Article.Application.UseCases.Requests.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class GetPaginatedByCategoryArticlesDTOToRequest : Profile
{
    public GetPaginatedByCategoryArticlesDTOToRequest()
    {
        CreateMap<GetPaginatedByCategoryArticlesRequestDTO, GetPaginatedByCategoryShortArticlesRequest>()
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(d => d.PageNo, opt => opt.MapFrom(src => src.PageNo))
            .ForMember(d => d.PageSize, opt => opt.MapFrom(src => src.PageSize));
    }
}
