using Article.Domain.Abstractions.Services;
using Article.Infrastructure.gRPC.Protos;
using Article.Infrastructure.Options.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace Article.Infrastructure.gRPC.Services.Clients;
public class UserService : IUserService
{
    private readonly User.UserClient _userClient;

    public UserService(IOptions<UrlsOption> urls)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        var chanel = GrpcChannel.ForAddress(urls.Value.IdentityUrl, new GrpcChannelOptions()
        {
            HttpHandler = handler
        });

        _userClient = new User.UserClient(chanel);
    }

    public async Task<bool> IsUserExist(string username, CancellationToken cancellationToken = default)
    {
        var response = await _userClient.IsUserExistByNameAsync(
            new IsUserExistByNameRequest() { Name = username },
            cancellationToken: cancellationToken);

        return response.IsExist;
    }
}
