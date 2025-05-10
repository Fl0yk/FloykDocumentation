using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using AutoMapper;
using Core.Models.Events;
using MassTransit;

namespace Article.Infrastructure.Consumers.Users;

public sealed class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserCreatedEventConsumer(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var user = _mapper.Map<User>(context.Message);

        await _unitOfWork.UserRepository.CreateAsync(user, context.CancellationToken);

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}
