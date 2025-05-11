using Core.Models;
using Identity.Application.Shared.Models.DTOs;
using MediatR;

namespace Identity.Application.UseCases.Query.Users;

public sealed class GetShortUsersInfoByPartialNameQuery : IRequest<PaginatedResult<ShortUserDto>>
{
    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public string PartialUsername { get; init; } = null!;
}
