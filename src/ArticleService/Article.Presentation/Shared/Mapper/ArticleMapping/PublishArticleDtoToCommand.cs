using Article.Application.UseCases.Comand.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class PublishArticleDtoToCommand : Profile
{
    public PublishArticleDtoToCommand()
    {
        CreateMap<PublishArticleRequestDTO, PublishArticleCommand>()
            .ForMember(d => d.ArticleId, opt => opt.MapFrom(src => src.ArticleId));
    }
}
