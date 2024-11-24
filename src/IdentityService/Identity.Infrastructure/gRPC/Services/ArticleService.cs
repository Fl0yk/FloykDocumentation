using AutoMapper;
using Grpc.Net.Client;
using Identity.Contracts.Abstractions.Services;
using Identity.DataAccess.Entities;
using Identity.Infrastructure.Shared.Options.Models;
using Microsoft.Extensions.Options;

using ArticleGrpc = Identity.Infrastructure.gRPC.Protos.Article;

namespace Identity.Infrastructure.gRPC.Services;

internal class ArticleService : IArticleService
{
    private readonly ArticleGrpc.ArticleClient _articleClient;
    private readonly IMapper _mapper;

    public ArticleService(IOptions<UrlsOption> urls, IMapper mapper)
    {
        var chanel = GrpcChannel.ForAddress(urls.Value.ArticleUrl);
        _articleClient = new ArticleGrpc.ArticleClient(chanel);
        _mapper = mapper;
    }

    public async Task<Article> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _articleClient.GetArticleByIdAsync(
            new Protos.GetArticleByIdRequest() { Id = id.ToString() },
            cancellationToken: cancellationToken);

        return _mapper.Map<Article>(response);
    }

    public async Task<bool> IsArticleExistAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _articleClient.IsArticleExistAsync(
            new Protos.IsArticleExistRequest() { Id = id.ToString() }, 
            cancellationToken:  cancellationToken);

        return response.IsExist;
    }
}
