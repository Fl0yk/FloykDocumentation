using MediatR;

namespace Identity.Application.UseCases.Command.Users;

public sealed class UpdateAvatarCommand : IRequest
{
    public string FileName { get; init; } = null!;

    public Stream ImageStream { get; init; } = null!;
}
