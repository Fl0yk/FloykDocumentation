using AutoMapper;
using Identity.DataAccess.Entities;

using GrpcResponse = Identity.Infrastructure.gRPC.Protos.GetArticleByIdResponse;

namespace Identity.Infrastructure.Shared.Mapper.ArticleMapping;

public class GetArticleByIdResponseToArticle : Profile
{
    public GetArticleByIdResponseToArticle()
    {
        CreateMap<GrpcResponse, Article>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Title));
    }
}
