using Article.Application.UseCases.Requests.Articles;
using Article.Domain.Entities;
using AutoMapper;

namespace Article.Application.Shared.Mapper.ArticleMapping;

public class AppendBlockRequestToBlock : Profile
{
    public AppendBlockRequestToBlock()
    {
        CreateMap<AppendBlockRequest, Block>()
            .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(d => d.Type, opt => opt.MapFrom(src => src.BlockType));
    }
}
