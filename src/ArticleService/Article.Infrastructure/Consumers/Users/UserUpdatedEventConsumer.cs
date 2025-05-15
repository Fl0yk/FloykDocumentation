using Article.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Models.Events;
using Core.Providers.Interfaces;
using MassTransit;

namespace Article.Infrastructure.Consumers.Users;

public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITransactionProvider _transactionProvider;

    public UserUpdatedEventConsumer(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITransactionProvider transactionProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _transactionProvider = transactionProvider;
    }

    public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
    {
        await _transactionProvider.OpenTransaction(context.CancellationToken);

        var user = await _unitOfWork.UserRepository.GetByIdAsync(context.Message.Id, context.CancellationToken);

        if (user is null)
        {
            throw new GuardNotFoundException($"User with id {context.Message.Id} not found during{nameof(UserUpdatedEventConsumer)}");
        }

        _mapper.Map(context.Message, user);

        await _unitOfWork.UserRepository.UpdateAsync(user, context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);

        await _transactionProvider.Commit(context.CancellationToken);
    }
}
