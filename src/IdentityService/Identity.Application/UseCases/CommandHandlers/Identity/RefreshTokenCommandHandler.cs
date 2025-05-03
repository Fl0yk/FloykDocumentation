using Core.Exceptions;
using Core.Providers.Interfaces;
using Identity.Application.Shared.Models;
using Identity.Application.UseCases.Command.Identity;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.UseCases.CommandHandlers.Identity;

internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AccessToken>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly ITransactionProvider _transactionProvider;

    public RefreshTokenCommandHandler(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        IJwtProvider jwtProvider, 
        ICurrentUserProvider currentUserProvider,
        ITransactionProvider transactionProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
        _currentUserProvider = currentUserProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task<AccessToken> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var user = _currentUserProvider.GetCurrentUser(request.JwtToken);

        if (user is null)
        {
            throw new GuardUnauthorizedException("User is not authenticated");
        }

        var dbUser = await _userManager.FindByEmailAsync(user.Email);

        if (dbUser is null || dbUser.Email != user.Email || dbUser.Id != user.Id)
        {
            throw new GuardArgumentException("Invalid user for refresh");
        }

        if (dbUser.RefreshTokenExpiry is null
            || dbUser.RefreshTokenExpiry <= DateTime.UtcNow
            || dbUser.RefreshToken is null)
        {
            //TODO: код для понимания на клиенте?
            throw new GuardArgumentException("Invalid refresh token");
        }

        var principals = await _signInManager.CreateUserPrincipalAsync(dbUser);

        string jwt = _jwtProvider.GenerateJwt(dbUser, principals.Claims);

        await _transactionProvider.Commit(cancellationToken);

        //TODO: токены вообще в бд сохраняются?
        return new AccessToken()
        {
            JwtToken = jwt,
            RefreshToken = dbUser.RefreshToken!,
            RefreshTokenExpiry = dbUser.RefreshTokenExpiry!.Value,
        };
    }
}
