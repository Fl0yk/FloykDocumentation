using Article.Application.UseCases.Query.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class GetPaginatedByDateArticlesDtoToQuery : Profile
{
    public GetPaginatedByDateArticlesDtoToQuery()
    {
        CreateMap<GetPaginatedByDateArticlesRequestDTO, GetPaginatedByDateShortArticlesQuery>()
            .ForMember(d => d.PageNo, opt => opt.MapFrom(src => src.PageNo))
            .ForMember(d => d.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(d => d.IsDocumentation, opt => opt.MapFrom(src => src.IsDocumentation))
            .ForMember(d => d.Categories, opt => opt.MapFrom(src => src.Categories));
    }
}
