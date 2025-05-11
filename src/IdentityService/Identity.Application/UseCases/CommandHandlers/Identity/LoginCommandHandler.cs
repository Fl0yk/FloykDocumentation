using Core.Exceptions;
using Core.Providers.Interfaces;
using Identity.Application.Shared.Models;
using Identity.Application.UseCases.Command.Identity;
using Identity.Domain.Abstractions.Providers;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.UseCases.CommandHandlers.Identity;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AccessToken>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly ITransactionProvider _transactionProvider;

    public LoginCommandHandler(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        IJwtProvider jwtProvider,
        ITransactionProvider transactionProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
        _transactionProvider = transactionProvider;
    }

    public async Task<AccessToken> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new GuardArgumentException("Invalid username or password");
        }

        var dbUser = await _userManager.FindByNameAsync(request.Username);

        if (dbUser is null)
        {
            throw new GuardArgumentException($"User with username {request.Username} not found");
        }

        if (!dbUser.EmailConfirmed)
        {
            throw new GuardUnauthorizedException($"Email not confirmed for user with id {dbUser.Id}");
        }

        var principals = await _signInManager.CreateUserPrincipalAsync(dbUser);

        string jwt = _jwtProvider.GenerateJwt(dbUser, principals.Claims);

        UpdateRefresh(dbUser);

        await _userManager.UpdateAsync(dbUser);

        await _transactionProvider.Commit(cancellationToken);

        return new()
        {
            JwtToken = jwt,
            RefreshToken = dbUser.RefreshToken!,
            RefreshTokenExpiry = dbUser.RefreshTokenExpiry!.Value,
        };
    }

    private void UpdateRefresh(User user)
    {
        string refresh = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);
    }
}
