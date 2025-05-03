using MediatR;

namespace Identity.Application.UseCases.Command.Users;

public sealed class FollowCommand : IRequest
{
    public Guid AuthorId { get; init; }
}
