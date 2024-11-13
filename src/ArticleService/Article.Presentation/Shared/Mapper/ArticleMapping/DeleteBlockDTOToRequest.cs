using Article.Application.UseCases.Requests.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;

namespace Article.Presentation.Shared.Mapper.ArticleMapping;

public class DeleteBlockDTOToRequest : Profile
{
    public DeleteBlockDTOToRequest()
    {
        CreateMap<DeleteBlockRequestDTO, DeleteBlockRequest>()
            .ForMember(d => d.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
            .ForMember(d => d.BlockId, opt => opt.MapFrom(src => src.BlockId))
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(src => src.AuthorName));
    }
}
