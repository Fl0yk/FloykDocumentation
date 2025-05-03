using Article.Application.UseCases.Query.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class GetPaginatedByCategoryArticlesDtoToQuery : Profile
{
    public GetPaginatedByCategoryArticlesDtoToQuery()
    {
        CreateMap<GetPaginatedByCategoryArticlesRequestDTO, GetPaginatedByCategoryShortArticlesQuery>()
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(d => d.PageNo, opt => opt.MapFrom(src => src.PageNo))
            .ForMember(d => d.PageSize, opt => opt.MapFrom(src => src.PageSize));
    }
}
