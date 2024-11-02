using Article.Application.UseCases.Requests.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class DeleteArticleDTOToRequest : Profile
{
    public DeleteArticleDTOToRequest()
    {
        CreateMap<DeleteArticleRequestDTO, DeleteArticleRequest>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.AuthorName));
    }
}
