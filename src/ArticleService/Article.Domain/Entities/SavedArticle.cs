using Core.Abstractions;

namespace Article.Domain.Entities;

public class SavedArticle : BaseEntity
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public Guid ArticleId { get; set; }
}
