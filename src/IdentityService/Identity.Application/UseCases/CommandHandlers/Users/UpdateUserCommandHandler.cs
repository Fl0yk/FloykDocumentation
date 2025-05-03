using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using Identity.Application.UseCases.Command.Users;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Repositories.Abstractions;
using MediatR;

namespace Identity.Application.UseCases.CommandHandlers.Users;
internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly ITransactionProvider _transactionProvider;

    public UpdateUserCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ICurrentUserProvider currentUserProvider,
        ITransactionProvider transactionProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var user = _currentUserProvider.GetCurrentUser();

        if (user is null)
        {
            throw new GuardUnauthorizedException("User is not authorize");
        }

        string oldUsername = user.Username;

        var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new GuardNotFoundException($"User with id {user.Id} was not found");
        }

        if (dbUser.UserName == oldUsername)
        {
            return;
        }

        var anotherUser = await _unitOfWork.UserRepository.GetUserByNameAsync(request.NewUsername, cancellationToken);

        if (anotherUser is not null)
        {
            throw new GuardArgumentException($"Username is already taken");
        }

        _mapper.Map(request, dbUser);

        await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
        //TODO: think with replication
        //await _publishEndpoint.Publish<UsernameUpdated>(new
        //{
        //    OldUsername = oldUsername,
        //    NewUsername = dbUser.UserName
        //}, cancellationToken);
    }
}
