using Article.Application.UseCases.Requests.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class CreateArticleDTOToRequest : Profile
{
    public CreateArticleDTOToRequest()
    {
        CreateMap<CreateArticleRequestDTO, CreateArticleRequest>()
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.CurrentUserName))
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
    }
}
