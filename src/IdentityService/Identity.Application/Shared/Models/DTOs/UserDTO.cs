namespace Identity.Application.Shared.Models.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Avatar { get; set; }

    public ICollection<FollowingDTO> Followings { get; set; } = [];

    public ICollection<SavedArticleDTO> SavedArticles { get; set; } = [];
}
