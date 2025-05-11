using Identity.Application.Shared.Models;
using MediatR;

namespace Identity.Application.UseCases.Command.Identity;

public sealed class RegistrationConfirmCommand : IRequest<AccessToken>
{
    public string Token { get; init; } = null!;

    public Guid UserId { get; init; }
}
