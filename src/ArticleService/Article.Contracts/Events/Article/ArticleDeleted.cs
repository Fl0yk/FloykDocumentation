using MassTransit;

namespace Article.Contracts.Events.Article;

[MessageUrn("article")]
[EntityName("article-deleted")]
public interface ArticleDeleted
{
    public Guid Id { get; set; }
}
