using MediatR;

namespace Identity.Application.UseCases.Command.Users;

public sealed class UpdateUserCommand : IRequest
{
    public string NewPublicUsername { get; init; } = null!;
}
