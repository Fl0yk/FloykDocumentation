using AutoMapper;
using Azure.Core;
using Identity.Application.Abstractions.Managers;
using Identity.Application.Abstractions.Providers;
using Identity.Application.Abstractions.Services;
using Identity.Application.Services.Requests.UserRequests;
using Identity.Application.Shared.Exceptions;
using Identity.Application.Shared.Mapper.UserMapping;
using Identity.Application.Shared.Models.DTOs;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Abstractions;

namespace Identity.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageManager _imageManager;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IMapper _mapper;

    public UserService(
        IUnitOfWork unitOfWork, 
        IImageManager imageManager, 
        ICurrentUserProvider currentUserProvider, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _imageManager = imageManager;
        _currentUserProvider = currentUserProvider;
        _mapper = mapper;
    }

    public async Task SaveArticleAsync(SaveArticleRequest articleRequest, CancellationToken cancellationToken = default)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new UnauthorizedException("User is not authorize");
        }

        var dbUser = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new BadRequestException("Invalid token");
        }

        if (dbUser.SavedArticles.Any(a => a.ArticleId == articleRequest.Id))
        {
            throw new BadRequestException("User already save this article");
        }

        dbUser.SavedArticles.Add(_mapper.Map<SavedArticle>(articleRequest));

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

        var dbUser = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new BadRequestException("Invalid token");
        }

        var dbAuthor = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByIdAsync(authorId, cancellationToken);

        if (dbAuthor is null)
        {
            throw new BadRequestException($"Author with id {authorId} was not found");
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
        var dbUser = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByNameAsync(username, cancellationToken);

        if (dbUser is null)
        {
            throw new BadRequestException($"User with username {username} was not found");
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

        var dbUser = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new BadRequestException("Invalid token");
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

    public async Task UnfollowAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new UnauthorizedException("User is not authorize");
        }

        var dbUser = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new BadRequestException("Invalid token");
        }

        var dbAuthor = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByIdAsync(authorId, cancellationToken);

        if (dbAuthor is null)
        {
            throw new BadRequestException($"Author with id {authorId} was not found");
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

        var dbUser = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new BadRequestException("Invalid token");
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

        var dbUser = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new BadRequestException("Invalid token");
        }

        var anotherUser = await _unitOfWork.UserRepository.FirstOrDefaultFullUserByNameAsync(updateUserRequest.NewUsername, cancellationToken);

        if (anotherUser is not null)
        {
            throw new BadRequestException($"Username is already taken");
        }

        _mapper.Map(updateUserRequest, dbUser);

        await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
