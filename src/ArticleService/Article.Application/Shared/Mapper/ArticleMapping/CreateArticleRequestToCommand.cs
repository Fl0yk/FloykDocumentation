using Article.Application.UseCases.Comand.Articles;
using AutoMapper;

namespace Article.Application.Shared.Mapper.ArticleMapping;

public class CreateArticleRequestToCommand : Profile
{
    public CreateArticleRequestToCommand() 
    {
        CreateMap<CreateArticleCommand, Domain.Entities.Article>()
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(d => d.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
    }
}
