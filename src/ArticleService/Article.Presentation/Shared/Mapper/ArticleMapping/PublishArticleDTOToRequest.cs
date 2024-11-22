using Article.Application.UseCases.Requests.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class PublishArticleDTOToRequest : Profile
{
    public PublishArticleDTOToRequest()
    {
        CreateMap<PublishArticleRequestDTO, PublishArticleRequest>()
            .ForMember(d => d.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.CurrentUserName));
    }
}
