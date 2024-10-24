using Identity.Application.Services.Requests.UserRequests;
using Identity.Application.Shared.Models.DTOs;

namespace Identity.Application.Abstractions.Services;

public interface IUserService
{
    public Task<UserDTO> GetUserByIdAsync(CancellationToken cancellationToken = default);

    public Task FollowAsync(Guid authorId, CancellationToken cancellationToken = default);

    public Task UnfollowAsync(Guid authorId, CancellationToken cancellationToken = default);

    public Task AddSavedArticleAsync(ArticleRequest articleRequest, CancellationToken cancellationToken = default);

    public Task RemoveSavedArticleAsync(ArticleRequest articleRequest, CancellationToken cancellationToken = default);

    public Task UpdateAvatarAsync(UpdateAvatarRequest updateAvatarRequest, CancellationToken cancellationToken = default);

    public Task UpdateUserAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default);
}
