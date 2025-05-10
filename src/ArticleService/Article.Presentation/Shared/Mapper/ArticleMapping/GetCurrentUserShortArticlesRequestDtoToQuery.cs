using Article.Application.UseCases.Query.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class GetCurrentUserShortArticlesRequestDtoToQuery : Profile
{
    public GetCurrentUserShortArticlesRequestDtoToQuery()
    {
        CreateMap<GetCurrentUserShortArticlesRequestDto, GetCurrentUserShortArticlesQuery>()
            .ForMember(d => d.PageNo, opt => opt.MapFrom(src => src.PageNo))
            .ForMember(d => d.PageSize, opt => opt.MapFrom(src => src.PageSize));
    }
}
