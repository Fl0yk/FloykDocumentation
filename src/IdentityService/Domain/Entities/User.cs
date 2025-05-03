using Core.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class User : IdentityUser<Guid>, IBaseEntity
{
    public string? RefreshToken { get; set; }

    public DateTimeOffset? RefreshTokenExpiry { get; set; }

    public string PublicUsername { get; set; } = null!;

    public string? Avatar {  get; set; }

    public ICollection<Following> Followings { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    //TODO: think about saved articles
    //public ICollection<SavedArticle> SavedArticles { get; set; } = [];
}
