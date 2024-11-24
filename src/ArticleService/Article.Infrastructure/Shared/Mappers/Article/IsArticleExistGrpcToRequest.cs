using Article.Infrastructure.gRPC.Protos;
using AutoMapper;

using ApplicationRequest = Article.Application.UseCases.Requests.Articles.GetArticleByIdRequest;

namespace Article.Infrastructure.Shared.Mappers.Article;

public class IsArticleExistGrpcToRequest : Profile
{
    public IsArticleExistGrpcToRequest()
    {
        CreateMap<IsArticleExistRequest, ApplicationRequest>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id));
    }
}
