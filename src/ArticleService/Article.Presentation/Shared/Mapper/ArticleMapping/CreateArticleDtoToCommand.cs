using Article.Application.UseCases.Comand.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class CreateArticleDtoToCommand : Profile
{
    public CreateArticleDtoToCommand()
    {
        CreateMap<CreateArticleRequestDTO, CreateArticleCommand>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(d => d.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription))
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
    }
}
