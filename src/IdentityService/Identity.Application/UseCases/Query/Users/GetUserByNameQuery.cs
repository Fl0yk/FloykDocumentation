using Identity.Application.Shared.Models.DTOs;
using MediatR;

namespace Identity.Application.UseCases.Query.Users;

//TODO: переписать на частичный поиск и несколько? (искать только с ролью автора или любого?)
public sealed class GetUserByNameQuery : IRequest<UserDTO>
{
    public string Username { get; set; } = null!;
}
