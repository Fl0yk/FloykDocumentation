using Identity.Application.Shared.Models.DTOs;
using MediatR;

namespace Identity.Application.UseCases.Query.Users;

public sealed class GetCurrentUserInfoQuery : IRequest<UserDTO>
{
    
}
