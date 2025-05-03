using MediatR;

namespace Identity.Application.UseCases.Command.Users;

public sealed class UnfollowCommand : IRequest
{
    public Guid AuthorId { get; init; }
}
