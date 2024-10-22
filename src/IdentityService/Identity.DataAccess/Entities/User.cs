using Microsoft.AspNetCore.Identity;

namespace Identity.DataAccess.Entities;

public class User : IdentityUser<Guid>
{
    public string? Avatar {  get; set; }

    public ICollection<Following> Followings { get; set; } = [];

    public ICollection<SavedArticle> SavedArticles { get; set; } = [];
}
