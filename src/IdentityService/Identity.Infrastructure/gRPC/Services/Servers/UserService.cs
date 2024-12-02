using Grpc.Core;
using Identity.Application.Abstractions.Services;
using Identity.Infrastructure.Protos;

namespace Identity.Infrastructure.gRPC.Services.Servers;

public class UserService : User.UserBase
{
    private readonly IUserService _userService;

    public UserService(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<IsUserExistResponse> IsUserExistById(IsUserExistByIdRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out Guid id))
        {
            return new IsUserExistResponse() { IsExist = false };
        }

        bool isExist = await _userService.IsUserExist(id);

        return new IsUserExistResponse() { IsExist = isExist };
    }

    public override async Task<IsUserExistResponse> IsUserExistByName(IsUserExistByNameRequest request, ServerCallContext context)
    {
        bool isExist = await _userService.IsUserExist(request.Name);

        return new IsUserExistResponse() { IsExist = isExist };
    }
}
