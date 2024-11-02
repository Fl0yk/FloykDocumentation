using Article.Application.UseCases.Requests.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class AppendBlockDTOToRequest : Profile
{
    public AppendBlockDTOToRequest()
    {
        CreateMap<AppendBlockRequestDTO, AppendBlockRequest>()
            .ForMember(d => d.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
            .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(d => d.BlockType, opt => opt.MapFrom(src => src.BlockType));
    }
}
