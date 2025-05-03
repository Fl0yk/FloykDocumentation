using Article.Application.UseCases.Comand.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class DeleteArticleDtoToCommand : Profile
{
    public DeleteArticleDtoToCommand()
    {
        CreateMap<DeleteArticleRequestDTO, DeleteArticleCommand>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id));
    }
}
