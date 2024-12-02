using AutoMapper;
using Identity.Application.Abstractions.Managers;
using Identity.Application.Abstractions.Providers;
using Identity.Application.Abstractions.Services;
using Identity.Application.Shared.Exceptions;
using Identity.Application.Shared.Models.DTOs;
using Identity.Application.Shared.Models.Requests.UserRequests;
using Identity.Contracts.Abstractions.Services;
using Identity.Contracts.Events.User;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Abstractions;
using MassTransit;

namespace Identity.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageManager _imageManager;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IArticleService _articleService;
    private readonly IMapper _mapper;

    public UserService(
        IUnitOfWork unitOfWork,
        IImageManager imageManager,
        ICurrentUserProvider currentUserProvider,
        IPublishEndpoint publishEndpoint,
        IArticleService articleService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _imageManager = imageManager;
        _currentUserProvider = currentUserProvider;
        _publishEndpoint = publishEndpoint;
        _articleService = articleService;
        _mapper = mapper;
    }

    public async Task SaveArticleAsync(SaveArticleRequest articleRequest, CancellationToken cancellationToken = default)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new UnauthorizedException("User is not authorize");
        }

        var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new NotFoundException($"User with id {user.Id} was not found");
        }

        if (dbUser.SavedArticles.Any(a => a.ArticleId == articleRequest.Id))
        {
            throw new BadRequestException("User already save this article");
        }

        bool isExist = await _articleService.IsArticleExistAsync(articleRequest.Id, cancellationToken);

        if (!isExist)
        {
            throw new NotFoundException($"Article with id {articleRequest.Id} was not found");
        }

        Article article = await _articleService.GetArticleByIdAsync(articleRequest.Id, cancellationToken);

        dbUser.SavedArticles.Add(_mapper.Map<SavedArticle>(article));

        await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task FollowAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new UnauthorizedException("User is not authorize");
        }

        var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new NotFoundException($"User with id {user.Id} was not found");
        }

        var dbAuthor = await _unitOfWork.UserRepository.GetUserByIdAsync(authorId, cancellationToken);

        if (dbAuthor is null)
        {
            throw new NotFoundException($"Author with id {authorId} was not found");
        }

        if (dbUser.Followings.Any(f => f.AuthorId == authorId))
        {
            throw new BadRequestException("User already follow on this author");
        }

        dbUser.Followings.Add(new() { Author = dbAuthor });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserDTO> GetUserByNameAsync(string username, CancellationToken cancellationToken = default)
    {
        var dbUser = await _unitOfWork.UserRepository.GetUserByNameAsync(username, cancellationToken);

        if (dbUser is null)
        {
            throw new NotFoundException($"User with username {username} was not found");
        }

        return _mapper.Map<UserDTO>(dbUser);
    }

    public async Task RemoveSavedArticleAsync(Guid articleId, CancellationToken cancellationToken = default)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new UnauthorizedException("User is not authorize");
        }

        var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new NotFoundException($"User with id {user.Id} was not found");
        }

        var savedArticle = dbUser.SavedArticles.FirstOrDefault(a => a.ArticleId == articleId);

        if (savedArticle is null)
        {
            throw new BadRequestException($"User don't have saved article with id {articleId}");
        }

        dbUser.SavedArticles.Remove(savedArticle);

        await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveSavedArticleForAllAsync(Guid articleId, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.UserRepository.DeleteSavedArticleForAllAsync(articleId, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UnfollowAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new UnauthorizedException("User is not authorize");
        }

        var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new NotFoundException($"User with id {user.Id} was not found");
        }

        var dbAuthor = await _unitOfWork.UserRepository.GetUserByIdAsync(authorId, cancellationToken);

        if (dbAuthor is null)
        {
            throw new NotFoundException($"Author with id {authorId} was not found");
        }

        var follow = dbUser.Followings.FirstOrDefault(f => f.AuthorId == authorId);

        if (follow is null)
        {
            throw new BadRequestException("User is not follow on this author");
        }

        dbUser.Followings.Remove(follow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAvatarAsync(UpdateAvatarRequest updateAvatarRequest, CancellationToken cancellationToken = default)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new UnauthorizedException("User is not authorize");
        }

        var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new NotFoundException($"User with id {user.Id} was not found");
        }

        string imagePath = await _imageManager.SaveImageAsync(updateAvatarRequest.ImageStream, updateAvatarRequest.FileName, dbUser.Avatar, cancellationToken);

        dbUser.Avatar = imagePath;

        await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new UnauthorizedException("User is not authorize");
        }

        string oldUsername = user.Username;

        var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new NotFoundException($"User with id {user.Id} was not found");
        }

        var anotherUser = await _unitOfWork.UserRepository.GetUserByNameAsync(updateUserRequest.NewUsername, cancellationToken);

        if (anotherUser is not null)
        {
            throw new BadRequestException($"Username is already taken");
        }

        _mapper.Map(updateUserRequest, dbUser);

        await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (dbUser.UserName != oldUsername)
        {
            await _publishEndpoint.Publish<UsernameUpdated>(new
            {
                OldUsername = oldUsername,
                NewUsername = dbUser.UserName
            }, cancellationToken);
        }
    }

    public Task<bool> IsUserExist(Guid userId, CancellationToken cancellationToken = default)
    {
        return _unitOfWork.UserRepository.IsUserExist(userId, cancellationToken);
    }

    public Task<bool> IsUserExist(string username, CancellationToken cancellationToken = default)
    {
        return _unitOfWork.UserRepository.IsUserExist(username, cancellationToken);
    }
}
