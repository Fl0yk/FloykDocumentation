using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using AutoMapper;
using Core.Models.Events;
using MassTransit;
using Core.Providers.Interfaces;

namespace Forum.Infrastructure.Consumers.Users;

public sealed class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITransactionProvider _transactionProvider;

    public UserCreatedEventConsumer(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITransactionProvider transactionProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _transactionProvider = transactionProvider;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        await _transactionProvider.OpenTransaction(context.CancellationToken);

        var user = _mapper.Map<User>(context.Message);

        await _unitOfWork.UserRepository.CreateAsync(user, context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);

        await _transactionProvider.Commit(context.CancellationToken);
    }
}
