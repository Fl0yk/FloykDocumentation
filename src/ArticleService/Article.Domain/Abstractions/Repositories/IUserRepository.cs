using Article.Domain.Entities;

namespace Article.Domain.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> WithSavedArticles(User? user, CancellationToken cancellationToken = default);

    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    Task CreateAsync(User user, CancellationToken cancellationToken = default);

    Task SaveArticle(SavedArticle savedArticle, CancellationToken cancellationToken = default);

    Task UnsaveArticle(SavedArticle savedArticle, CancellationToken cancellationToken = default);

    Task RemoveSavedArticlesByArticleAsync(Guid articleId, CancellationToken cancellationToken = default);
}
