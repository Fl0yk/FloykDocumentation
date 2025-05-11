using AutoMapper;
using Core.Exceptions;
using Identity.Application.Shared.Models.DTOs;
using Identity.Application.UseCases.Query.Users;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Repositories.Abstractions;
using MediatR;

namespace Identity.Application.UseCases.QueryHandlers.Users;

public sealed class GetCurrentUserInfoQueryHandler : IRequestHandler<GetCurrentUserInfoQuery, UserDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IMapper _mapper;

    public GetCurrentUserInfoQueryHandler(
        IUnitOfWork unitOfWork, 
        ICurrentUserProvider currentUserProvider,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserProvider = currentUserProvider;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetCurrentUserInfoQuery request, CancellationToken cancellationToken)
    {
        var initiator = _currentUserProvider.GetCurrentUser();

        if (initiator is null)
        {
            throw new GuardForbiddenException("Current user is null");
        }

        var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(initiator.Id, cancellationToken);

        if (dbUser is null)
        {
            throw new GuardNotFoundException($"User with id {initiator.Id} was not found");
        }

        return _mapper.Map<UserDTO>(dbUser);
    }
}
