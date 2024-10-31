using Article.Domain.Models;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Domain.Abstractions.Repositories;

public interface IArticleRepository
{
    public Task<IEnumerable<ArticleModel>> GetPaginatedByDateWithoutBlocksArticles(int pageNo, int pageSize, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ArticleModel>> GetPaginatedByCategoryWithoutBlocksArticles(Guid categoryId, int pageNo, int pageSize, CancellationToken cancellationToken = default);

    public Task<ArticleModel?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task CreateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default);

    public Task UpdateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default);

    public Task DeleteArticleAsync(ArticleModel article, CancellationToken cancellationToken = default);
}
