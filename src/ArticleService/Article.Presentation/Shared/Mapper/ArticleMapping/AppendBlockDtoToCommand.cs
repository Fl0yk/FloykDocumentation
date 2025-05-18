using Article.Application.UseCases.Comand.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class AppendBlockDtoToCommand : Profile
{
    public AppendBlockDtoToCommand()
    {
        CreateMap<AppendBlockRequestDTO, AppendBlockCommand>()
            .ForMember(d => d.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
            .ForMember(d => d.Blocks,opt => opt.Ignore());
    }
}
