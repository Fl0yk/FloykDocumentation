using AutoMapper;
using Core.Exceptions;
using Identity.Application.Shared.Models.DTOs;
using Identity.Application.UseCases.Query.Users;
using Identity.Domain.Repositories.Abstractions;
using MediatR;

namespace Identity.Application.UseCases.QueryHandlers.Users;
internal sealed class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, UserDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByNameQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    {
        var dbUser = await _unitOfWork.UserRepository.GetUserByNameAsync(request.Username, cancellationToken);

        if (dbUser is null)
        {
            throw new GuardNotFoundException($"User with username {request.Username} was not found");
        }

        return _mapper.Map<UserDTO>(dbUser);
    }
}
