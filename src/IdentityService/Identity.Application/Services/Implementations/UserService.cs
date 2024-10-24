using Identity.Application.Abstractions.Managers;
using Identity.Application.Abstractions.Providers;
using Identity.Application.Abstractions.Services;
using Identity.Application.Services.Requests.UserRequests;
using Identity.Application.Shared.Models.DTOs;
using Identity.DataAccess.Repositories.Abstractions;

namespace Identity.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _UserRepository;
    private readonly IImageManager _imageManager;
    private readonly ICurrentUserProvider _currentUserProvider;

    public UserService(IUserRepository userRepository, IImageManager imageManager, ICurrentUserProvider currentUserProvider)
    {
        _UserRepository = userRepository;
        _imageManager = imageManager;
        _currentUserProvider = currentUserProvider;
    }

    public Task AddSavedArticleAsync(ArticleRequest articleRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task FollowAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> GetUserByIdAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveSavedArticleAsync(ArticleRequest articleRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UnfollowAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAvatarAsync(UpdateAvatarRequest updateAvatarRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
