using Article.Application.UseCases.Query.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class GetPaginatedByAuthorArticlesDtoToQuery : Profile
{
    public GetPaginatedByAuthorArticlesDtoToQuery()
    {
        CreateMap<GetPaginatedByAuthorArticlesRequestDTO, GetPaginatedByAuthorNameShortArticlesQuery>()
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(d => d.PageNo, opt => opt.MapFrom(src => src.PageNo))
            .ForMember(d => d.PageSize, opt => opt.MapFrom(src => src.PageSize));
    }
}
