using MediatR;

namespace Identity.Application.UseCases.Command.Identity;

public sealed class AddUserToRoleCommand : IRequest
{
    public string Username { get; set; } = null!;

    public string RoleName { get; set; } = null!;
}
