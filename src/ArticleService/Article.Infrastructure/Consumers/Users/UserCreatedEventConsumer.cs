using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using AutoMapper;
using Core.Models.Events;
using Core.Providers.Interfaces;
using MassTransit;

namespace Article.Infrastructure.Consumers.Users;

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
