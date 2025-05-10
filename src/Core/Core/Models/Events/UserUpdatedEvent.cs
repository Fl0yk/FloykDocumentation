namespace Core.Models.Events;

public sealed class UserUpdatedEvent
{
    public Guid Id { get; set; }

    public string PublicUsername { get; set; } = null!;
}
