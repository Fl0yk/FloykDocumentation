using Core.Exceptions;
using Core.Providers.Interfaces;
using Identity.Application.UseCases.Command.Users;
using Identity.Domain.Abstractions.Managers;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Repositories.Abstractions;
using MediatR;

namespace Identity.Application.UseCases.CommandHandlers.Users;

internal sealed class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageManager _imageManager;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly ITransactionProvider _transactionProvider;

    public UpdateAvatarCommandHandler(
        IUnitOfWork unitOfWork, 
        IImageManager imageManager, 
        ICurrentUserProvider currentUserProvider,
        ITransactionProvider transactionProvider)
    {
        _unitOfWork = unitOfWork;
        _imageManager = imageManager;
        _currentUserProvider = currentUserProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new GuardUnauthorizedException("User is not authorize");
        }

        var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new GuardNotFoundException($"User with id {user.Id} was not found");
        }
        string imagePath = await _imageManager.SaveImageAsync(request.ImageStream, request.FileName, dbUser.Avatar, cancellationToken);
        dbUser.Avatar = imagePath;

        await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
    }
}
