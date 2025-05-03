using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Domain.Abstractions.Repositories;

public interface IArticleRepository
{
    public Task<IEnumerable<ArticleModel>> GetPaginatedByDateWithoutBlocksArticlesAsync(int pageNo, int pageSize, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ArticleModel>> GetPaginatedByCategoryWithoutBlocksArticlesAsync(Guid categoryId, int pageNo, int pageSize, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ArticleModel>> GetPaginatedByAuthorWithoutBlocksArticlesAsync(Guid authorId, int pageNo, int pageSize, CancellationToken cancellationToken = default);

    public Task<long> CountAsync(CancellationToken cancellationToken = default);

    public Task<long> CountByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);

    public Task<long> CountByAuthorAsync(Guid authorId,  CancellationToken cancellationToken = default);

    public Task<ArticleModel?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task CreateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default);

    public Task UpdateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default);

    public Task DeleteArticleAsync(ArticleModel article, CancellationToken cancellationToken = default);
}
