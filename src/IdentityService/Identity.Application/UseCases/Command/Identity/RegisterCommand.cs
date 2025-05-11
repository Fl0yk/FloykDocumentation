using Identity.Application.Shared.Models;
using MediatR;

namespace Identity.Application.UseCases.Command.Identity;

public sealed class RegisterCommand : IRequest
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
