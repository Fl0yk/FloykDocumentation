using Forum.Domain.Abstractions.Services;
using Forum.Infrastructure.gRPC.Protos;
using Forum.Infrastructure.Options.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace Forum.Infrastructure.gRPC.Services;
public class UserService : IUserService
{
    private readonly User.UserClient _userClient;

    public UserService(IOptions<UrlsOption> urls)
    {
        var chanel = GrpcChannel.ForAddress(urls.Value.IdentityUrl);
        _userClient = new User.UserClient(chanel);
    }

    public async Task<bool> IsUserExist(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _userClient.IsUserExistByIdAsync(
            new IsUserExistByIdRequest() { Id = id.ToString() },
            cancellationToken: cancellationToken);

        return response.IsExist;
    }
}
