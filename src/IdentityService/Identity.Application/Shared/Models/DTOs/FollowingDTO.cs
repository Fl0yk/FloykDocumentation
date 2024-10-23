namespace Identity.Application.Shared.Models.DTOs;

public class FollowingDTO
{
    public Guid AuthorId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public DateTime DateOfFollow { get; set; }
}
