namespace Core.Models.Events;

public sealed class ArticleApprovedEvent
{
    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;
}
