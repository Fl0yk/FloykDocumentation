using Article.Application.UseCases.Requests.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class GetPaginatedByAuthorArticlesDTOToRequest : Profile
{
    public GetPaginatedByAuthorArticlesDTOToRequest()
    {
        CreateMap<GetPaginatedByAuthorArticlesRequestDTO, GetPaginatedByAuthorNameShortArticlesRequest>()
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(d => d.PageNo, opt => opt.MapFrom(src => src.PageNo))
            .ForMember(d => d.PageSize, opt => opt.MapFrom(src => src.PageSize));
    }
}
