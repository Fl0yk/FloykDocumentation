using AutoMapper;
using Core.Exceptions;
using Core.Models.Events;
using Core.Providers.Interfaces;
using Identity.Application.UseCases.Command.Users;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Repositories.Abstractions;
using MassTransit;
using MediatR;

namespace Identity.Application.UseCases.CommandHandlers.Users;
internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateUserCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ICurrentUserProvider currentUserProvider,
        ITransactionProvider transactionProvider,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
        _transactionProvider = transactionProvider;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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

        _mapper.Map(request, dbUser);

        await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(new UserUpdatedEvent()
        {
            Id = dbUser.Id,
            PublicUsername = dbUser.PublicUsername
        }, cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
    }
}
