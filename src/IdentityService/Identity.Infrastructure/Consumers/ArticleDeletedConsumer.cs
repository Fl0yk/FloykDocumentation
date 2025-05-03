using Identity.Application.Abstractions.Services;
using Identity.Contracts.Events.Article;
using MassTransit;

namespace Identity.Infrastructure.Consumers;

public class ArticleDeletedConsumer : IConsumer<ArticleDeleted>
{
    private readonly IUserService _userService;

    public ArticleDeletedConsumer(IUserService userService)
    {
        _userService = userService;
    }

    public async Task Consume(ConsumeContext<ArticleDeleted> context)
    {
        await _userService.RemoveSavedArticleForAllAsync(context.Message.Id, context.CancellationToken);
    }
}
