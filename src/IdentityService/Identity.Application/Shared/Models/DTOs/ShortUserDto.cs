namespace Identity.Application.Shared.Models.DTOs;

public sealed class ShortUserDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string PublicUsername { get; set; } = null!;

    public string? Avatar { get; set; }
}
