using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Identity.Application.Shared.Models.DTOs;
using Identity.Application.UseCases.Query.Users;
using Identity.Domain.Repositories.Abstractions;
using MediatR;

namespace Identity.Application.UseCases.QueryHandlers.Users;

public sealed class GetShortUsersInfoByPartialNameQueryHandler : IRequestHandler<GetShortUsersInfoByPartialNameQuery, PaginatedResult<ShortUserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetShortUsersInfoByPartialNameQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ShortUserDto>> Handle(GetShortUsersInfoByPartialNameQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.UserRepository.GetPaginatedUsersByPartialUsernameAsync(request.PageNo, request.PageSize, request.PartialUsername, cancellationToken);

        if (!users.Any())
        {
            throw new GuardArgumentException("Get an empty articles page");
        }

        long count = await _unitOfWork.UserRepository.GetCountByPartialNameAsync(request.PartialUsername, cancellationToken);

        return new()
        {
            Items = _mapper.Map<IEnumerable<ShortUserDto>>(users),
            TotalPages = (int)Math.Ceiling((double)count / request.PageSize),
            CurrentPage = request.PageNo,
            PageSize = request.PageSize
        };
    }
}
