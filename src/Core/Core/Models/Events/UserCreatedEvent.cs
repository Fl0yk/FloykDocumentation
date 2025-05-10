namespace Core.Models.Events;

public sealed class UserCreatedEvent
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string NormalizedUsername { get; set; } = null!;

    public string PublicUsername { get; set; } = null!;
}
