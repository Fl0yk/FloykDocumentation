using Article.Application.UseCases.Comand.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class UpdateArticleRequestDtoToCommand : Profile
{
    public UpdateArticleRequestDtoToCommand()
    {
        CreateMap<UpdateArticleRequestDTO, UpdateArticleCommand>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.NewShortDescription, opt => opt.MapFrom(src => src.NewShortDescription))
            .ForMember(d => d.NewTitle, opt => opt.MapFrom(src => src.NewTitle));
    }
}
