using MediatR;

namespace Identity.Application.UseCases.Command.Identity;

public sealed class AddUserToRoleCommand : IRequest
{
    public Guid UserId { get; set; }

    public string RoleName { get; set; } = null!;
}
