using Identity.Application.Shared.Models.DTOs;
using Identity.Application.Shared.Models.Requests.UserRequests;

namespace Identity.Application.Abstractions.Services;

public interface IUserService
{
    public Task<UserDTO> GetUserByNameAsync(string username, CancellationToken cancellationToken = default);

    public Task FollowAsync(Guid authorId, CancellationToken cancellationToken = default);

    public Task UnfollowAsync(Guid authorId, CancellationToken cancellationToken = default);

    public Task SaveArticleAsync(SaveArticleRequest articleRequest, CancellationToken cancellationToken = default);

    public Task RemoveSavedArticleAsync(Guid articleId, CancellationToken cancellationToken = default);

    public Task RemoveSavedArticleForAllAsync(Guid articleId, CancellationToken cancellationToken = default);

    public Task UpdateAvatarAsync(UpdateAvatarRequest updateAvatarRequest, CancellationToken cancellationToken = default);

    public Task UpdateUserAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default);

    public Task<bool> IsUserExist(Guid userId, CancellationToken cancellationToken = default);

    public Task<bool> IsUserExist(string username, CancellationToken cancellationToken = default);
}
