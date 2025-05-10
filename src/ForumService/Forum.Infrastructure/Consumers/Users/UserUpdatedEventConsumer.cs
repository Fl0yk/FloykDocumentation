using Forum.Domain.Abstractions.Repositories;
using AutoMapper;
using Core.Exceptions;
using Core.Models.Events;
using MassTransit;

namespace Forum.Infrastructure.Consumers.Users;

public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserUpdatedEventConsumer(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(context.Message.Id, context.CancellationToken);

        if (user is null)
        {
            throw new GuardNotFoundException($"User with id {context.Message.Id} not found during{nameof(UserUpdatedEventConsumer)}");
        }

        _mapper.Map(context.Message, user);

        await _unitOfWork.UserRepository.UpdateAsync(user, context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}
