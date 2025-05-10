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
            .ForMember(d => d.Data, opt => opt.MapFrom(src => src.Text))
            .ForMember(d => d.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(d => d.DeletedAt, opt => opt.MapFrom(src => src.DeletedAt))
            .ForMember(d => d.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));
    }
}
