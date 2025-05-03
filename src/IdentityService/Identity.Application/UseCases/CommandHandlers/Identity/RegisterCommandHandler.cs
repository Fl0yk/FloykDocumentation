using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using Identity.Application.Shared.Models;
using Identity.Application.UseCases.Command.Identity;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.UseCases.CommandHandlers.Identity;

internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, AccessToken>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITransactionProvider _transactionProvider;

    public RegisterCommandHandler(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        IMapper mapper, 
        IJwtProvider jwtProvider,
        ITransactionProvider transactionProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _jwtProvider = jwtProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task<AccessToken> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var dbUser = await _userManager.FindByEmailAsync(request.Email);

        if (dbUser is not null)
        {
            throw new GuardArgumentException($"User with email {request.Email} already exists");
        }

        dbUser = await _userManager.FindByNameAsync(request.Username);

        if (dbUser is not null)
        {
            throw new GuardArgumentException($"User with username {request.Username} already exists");
        }

        User user = _mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new GuardArgumentException(string.Join('\n', result.Errors));
        }

        var principals = await _signInManager.CreateUserPrincipalAsync(user);

        string jwt = _jwtProvider.GenerateJwt(user, principals.Claims);

        UpdateRefresh(user);

        await _userManager.UpdateAsync(user);

        await _transactionProvider.Commit(cancellationToken);

        return new()
        {
            JwtToken = jwt,
            RefreshToken = user.RefreshToken!,
            RefreshTokenExpiry = user.RefreshTokenExpiry!.Value,
        };
    }

    private void UpdateRefresh(User user)
    {
        string refresh = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);
    }
}
