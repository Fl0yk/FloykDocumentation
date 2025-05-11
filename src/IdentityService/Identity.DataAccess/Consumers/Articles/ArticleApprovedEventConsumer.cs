using Core.Constants;
using Core.Exceptions;
using Core.Models.Events;
using Core.Providers.Interfaces;
using Identity.Domain.Abstractions.Managers;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Consumers.Articles;

public sealed class ArticleApprovedEventConsumer : IConsumer<ArticleApprovedEvent>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ITransactionProvider _transactionProvider;
    private readonly IEmailManager _emailManager;

    public ArticleApprovedEventConsumer(
        UserManager<User> userManager, 
        RoleManager<IdentityRole<Guid>> roleManager, 
        ITransactionProvider transactionProvider,
        IEmailManager emailManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _transactionProvider = transactionProvider;
        _emailManager = emailManager;
    }

    public async Task Consume(ConsumeContext<ArticleApprovedEvent> context)
    {
        await _transactionProvider.OpenTransaction(context.CancellationToken);

        var dbUser = await _userManager.FindByIdAsync(context.Message.UserId.ToString());

        if (dbUser is null)
        {
            throw new GuardArgumentException($"User with user id {context.Message.UserId} was not found");
        }

        var role = await _roleManager.FindByNameAsync(Roles.Author);

        if (role is null)
        {
            throw new GuardArgumentException($"Role with name {Roles.Author} was not found");
        }

        var result = await _userManager.AddToRoleAsync(dbUser, role.Name!);

        if (!result.Succeeded)
        {
            throw new GuardArgumentException(string.Join('\n', result.Errors));
        }

        await _emailManager.SendArticleApprovedEmailAsync(dbUser.Email!, dbUser.UserName!, context.Message.Title);

        await _transactionProvider.Commit(context.CancellationToken);
    }
}
