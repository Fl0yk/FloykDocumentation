using Identity.Application.Shared.Models;
using MediatR;

namespace Identity.Application.UseCases.Command.Identity;

public sealed class LoginCommand : IRequest<AccessToken>
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
