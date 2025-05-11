using AutoMapper;
using Core.Exceptions;
using Core.Models.Events;
using Core.Providers.Interfaces;
using Identity.Application.Shared.Models;
using Identity.Application.UseCases.Command.Identity;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.UseCases.CommandHandlers.Identity;

public sealed class RegistrationConfirmCommandHandler : IRequestHandler<RegistrationConfirmCommand, AccessToken>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IPublishEndpoint _publishEndpoint;

    public RegistrationConfirmCommandHandler(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        IMapper mapper, 
        IJwtProvider jwtProvider, 
        ITransactionProvider transactionProvider, 
        IPublishEndpoint publishEndpoint)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _jwtProvider = jwtProvider;
        _transactionProvider = transactionProvider;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<AccessToken> Handle(RegistrationConfirmCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            throw new GuardArgumentException($"User with id {request.UserId} not found");
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token.Replace(' ', '+'));

        if (!result.Succeeded)
        {
            throw new GuardArgumentException(string.Join('\n', result.Errors.Select(x => x.Description)));
        }

        var principals = await _signInManager.CreateUserPrincipalAsync(user);

        string jwt = _jwtProvider.GenerateJwt(user, principals.Claims);

        string refresh = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);

        await _userManager.UpdateAsync(user);

        await _transactionProvider.Commit(cancellationToken);

        await _publishEndpoint.Publish(_mapper.Map<UserCreatedEvent>(user), cancellationToken);

        return new AccessToken()
        {
            JwtToken = jwt,
            RefreshToken = user.RefreshToken!,
            RefreshTokenExpiry = user.RefreshTokenExpiry!.Value,
        };
    }
}
