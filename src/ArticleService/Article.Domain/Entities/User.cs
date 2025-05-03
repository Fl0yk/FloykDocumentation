using Core.Abstractions;

namespace Article.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = null!;

    public string NormalizedUsername { get; set; } = null!;

    public string PublicUsername { get; set; } = null!;

    public IEnumerable<SavedArticle> SavedArticles { get; set; } = null!;
}
