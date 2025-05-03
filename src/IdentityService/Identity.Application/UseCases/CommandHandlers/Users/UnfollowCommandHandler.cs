using Core.Exceptions;
using Core.Providers.Interfaces;
using Identity.Application.UseCases.Command.Users;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Repositories.Abstractions;
using MediatR;

namespace Identity.Application.UseCases.CommandHandlers.Users;

internal sealed class UnfollowCommandHandler : IRequestHandler<UnfollowCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly ITransactionProvider _transactionProvider;

    public UnfollowCommandHandler(
        IUnitOfWork unitOfWork, 
        ICurrentUserProvider currentUserProvider,
        ITransactionProvider transactionProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUserProvider = currentUserProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task Handle(UnfollowCommand request, CancellationToken cancellationToken)
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

        var dbAuthor = await _unitOfWork.UserRepository.GetUserByIdAsync(request.AuthorId, cancellationToken);

        if (dbAuthor is null)
        {
            throw new GuardNotFoundException($"Author with id {request.AuthorId} was not found");
        }

        var follow = dbUser.Followings.FirstOrDefault(f => f.AuthorId == request.AuthorId);

        if (follow is null)
        {
            throw new GuardArgumentException("User is not follow on this author");
        }

        dbUser.Followings.Remove(follow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
    }
}
