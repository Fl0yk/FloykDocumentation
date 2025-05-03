namespace Identity.Application.Shared.Models.DTOs;

public class FollowingDTO
{
    public Guid AuthorId { get; set; }

    public string AuthorName { get; set; } = null!;

    public DateTimeOffset DateOfFollow { get; set; }
}
