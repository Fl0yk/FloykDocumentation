using Article.Infrastructure.gRPC.Protos;
using AutoMapper;
using Grpc.Core;
using MediatR;

using ApplicationRequests = Article.Application.UseCases.Requests.Articles;
using ArticlegRPC = Article.Infrastructure.gRPC.Protos.Article;

namespace Article.Infrastructure.gRPC.Services.Servers;

public class ArticleService : ArticlegRPC.ArticleBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ArticleService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async override Task<IsArticleExistResponse> IsArticleExist(IsArticleExistRequest request, ServerCallContext context)
    {

        if (!Guid.TryParse(request.Id, out var id))
        {
            return new IsArticleExistResponse() { IsExist = false };
        }

        var isExist = await _mediator.Send(
                        new ApplicationRequests.IsArticleExistByIdRequest(id),
                        context.CancellationToken);

        return new IsArticleExistResponse() { IsExist = isExist };
    }

    public override async Task<GetArticleByIdResponse> GetArticleById(GetArticleByIdRequest request, ServerCallContext context)
    {
        var appRequest = _mapper.Map<ApplicationRequests.GetArticleByIdRequest>(request);

        var appResponse = await _mediator.Send(appRequest, context.CancellationToken);

        return _mapper.Map<GetArticleByIdResponse>(appResponse);
    }
}
