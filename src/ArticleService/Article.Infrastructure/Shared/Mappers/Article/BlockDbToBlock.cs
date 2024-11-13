using Article.Domain.Entities;
using Article.Infrastructure.Shared.Models;
using AutoMapper;

namespace Article.Infrastructure.Shared.Mappers.Article;

public class BlockDbToBlock : Profile
{
    public BlockDbToBlock()
    {
        CreateMap<BlockDb, Block>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(d => d.Type, opt => opt.MapFrom(src => src.Type));
    }
}
