using Article.Application.UseCases.Comand.Articles;
using Article.Domain.Entities;
using AutoMapper;

namespace Article.Application.Shared.Mapper.ArticleMapping;

public class AppendBlockRequestToBlock : Profile
{
    public AppendBlockRequestToBlock()
    {
        CreateMap<BlockInfo, Block>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(d => d.Type, opt => opt.MapFrom(src => src.BlockType));
    }
}