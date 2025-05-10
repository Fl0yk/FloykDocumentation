using Core.Exceptions;
using Core.Providers.Interfaces;
using Identity.Application.UseCases.Command.Identity;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.UseCases.CommandHandlers.Identity;

internal sealed class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ITransactionProvider _transactionProvider;

    public AddUserToRoleCommandHandler(
        UserManager<User> userManager, 
        RoleManager<IdentityRole<Guid>> roleManager,
        ITransactionProvider transactionProvider)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _transactionProvider = transactionProvider;
    }

    public async Task Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    {
        await _transactionProvider.OpenTransaction(cancellationToken);

        var dbUser = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (dbUser is null)
        {
            throw new GuardArgumentException($"User with username {request.UserId} was not found");
        }

        var role = await _roleManager.FindByNameAsync(request.RoleName);

        if (role is null)
        {
            throw new GuardArgumentException($"Role with name {request.RoleName} was not found");
        }

        var result = await _userManager.AddToRoleAsync(dbUser, role.Name!);

        if (!result.Succeeded)
        {
            throw new GuardArgumentException(string.Join('\n', result.Errors));
        }

        await _transactionProvider.Commit(cancellationToken);
    }
}
