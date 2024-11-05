using Identity.Application.Shared.Models;
using Identity.Application.Shared.Models.Requests.IdentityRequests;

namespace Identity.Application.Abstractions.Services;

public interface IIdentityService
{
    public Task<AccessToken> RegistrationAsync(RegistrationUserRequest registrationRequest, CancellationToken cancellationToken = default);

    public Task<AccessToken> LoginAsync(LoginUserRequest loginRequest, CancellationToken cancellationToken = default);

    public Task<AccessToken> ReefreshAsync(RefreshTokenRequest refreshRequest, CancellationToken cancellationToken = default);

    public Task AddUserToRoleAsync(AddUserToRoleRequest userToRoleRequest, CancellationToken cancellationToken = default);
}
