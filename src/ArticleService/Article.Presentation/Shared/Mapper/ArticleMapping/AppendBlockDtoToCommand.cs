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
            .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(d => d.BlockType, opt => opt.MapFrom(src => src.BlockType));
    }
}
