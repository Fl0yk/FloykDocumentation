using Article.Infrastructure.gRPC.Protos;
using Article.Application.Shared.Exceptions;
using AutoMapper;
using Grpc.Core;
using MediatR;

using ArticlegRPC = Article.Infrastructure.gRPC.Protos.Article;
using ApplicationRequest = Article.Application.UseCases.Requests.Articles.GetArticleByIdRequest;

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
        try
        {
            await _mediator.Send(
                        _mapper.Map<ApplicationRequest>(request),
                        context.CancellationToken);
        }
        catch (NotFoundException)
        {
            return new IsArticleExistResponse() { IsExist = false };
        }

        return new IsArticleExistResponse() { IsExist = true };
    }

    public override async Task<GetArticleByIdResponse> GetArticleById(GetArticleByIdRequest request, ServerCallContext context)
    {
        var appRequest = _mapper.Map<ApplicationRequest>(request);

        var appResponse = await _mediator.Send(appRequest, context.CancellationToken);

        return _mapper.Map<GetArticleByIdResponse>(appResponse);
    }
}
