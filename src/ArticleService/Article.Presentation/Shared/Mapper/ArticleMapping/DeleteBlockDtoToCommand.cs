using Article.Application.UseCases.Comand.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class DeleteBlockDtoToCommand : Profile
{
    public DeleteBlockDtoToCommand()
    {
        CreateMap<DeleteBlockRequestDTO, DeleteBlockCommand>()
            .ForMember(d => d.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
            .ForMember(d => d.BlockId, opt => opt.MapFrom(src => src.BlockId));
    }
}
