using AutoMapper;
using Core.Exceptions;
using Core.Providers.Interfaces;
using Identity.Application.UseCases.Command.Identity;
using Identity.Domain.Abstractions.Managers;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.UseCases.CommandHandlers.Identity;

internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IEmailManager _emailManager;

    public RegisterCommandHandler(
        UserManager<User> userManager, 
        IMapper mapper, 
        ITransactionProvider transactionProvider,
        IEmailManager emailManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _transactionProvider = transactionProvider;
        _emailManager = emailManager;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var dbUser = await _userManager.FindByNameAsync(request.Username);

        if (dbUser is not null && dbUser.EmailConfirmed)
        {
            throw new GuardArgumentException($"User with username {request.Username} already exists");
        }
        else if (dbUser is not null)
        {
            await CheckEmail(request.Email, cancellationToken);

            await _userManager.SetEmailAsync(dbUser, request.Email);

            await _userManager.UpdateAsync(dbUser);

            await SendEmail(dbUser, request.Email, cancellationToken);

            await _transactionProvider.Commit(cancellationToken);

            return;
        }

        await CheckEmail(request.Email, cancellationToken);

        User user = _mapper.Map<User>(request);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new GuardArgumentException(string.Join('\n', result.Errors));
        }

        await SendEmail(user, request.Email, cancellationToken);

        await _transactionProvider.Commit(cancellationToken);
    }

    private async Task SendEmail(User user, string email, CancellationToken cancellationToken)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        await _emailManager.SendRegistrationCompleteEmailAsync(email, token, user.Id);
    }

    private async Task CheckEmail(string email, CancellationToken cancellationToken)
    {
        var dbUser = await _userManager.FindByEmailAsync(email);

        if (dbUser is not null && dbUser.EmailConfirmed)
        {
            throw new GuardArgumentException($"User with email {email} already exists");
        }
    }
}
