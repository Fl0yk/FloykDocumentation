using Identity.Application.Shared.Models;
using MediatR;

namespace Identity.Application.UseCases.Command.Identity;

public sealed class RefreshTokenCommand : IRequest<AccessToken>
{
    public string JwtToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
