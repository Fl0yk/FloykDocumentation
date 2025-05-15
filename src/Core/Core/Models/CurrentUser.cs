namespace Core.Models;

public sealed class CurrentUser
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string PublicUsername { get; set; } = null!;

    public string Email { get; set; } = null!;

    public IEnumerable<string> Roles { get; set; } = null!;
}
