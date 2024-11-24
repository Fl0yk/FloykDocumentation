using AutoMapper;

using gRPCRequest = Article.Infrastructure.gRPC.Protos.GetArticleByIdRequest;
using ApplicationRequest = Article.Application.UseCases.Requests.Articles.GetArticleByIdRequest;

namespace Article.Infrastructure.Shared.Mappers.Article;

public class GetArticleByIdGrpcToRequest : Profile
{
    public GetArticleByIdGrpcToRequest()
    {
        CreateMap<gRPCRequest, ApplicationRequest>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id));
    }
}
