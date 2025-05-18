using Article.Domain.Entities;
using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Domain.Abstractions.Repositories;

public interface IArticleRepository
{
    public Task<IEnumerable<ArticleModel>> GetArticlesByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ArticleModel>> GetShouldBeApprovedArticlesAsync(CancellationToken cancellationToken = default);

    public Task<IEnumerable<ArticleModel>> GetPaginatedByDateWithoutBlocksArticlesAsync(int pageNo, int pageSize, bool? isDocumentation, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ArticleModel>> GetPaginatedByDateWithoutBlocksArticlesAsync(int pageNo, int pageSize, IEnumerable<Guid> categoriesId, bool? isDocumentation, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ArticleModel>> GetPopularPaginatedWithoutBlocksArticlesAsync(int pageNo, int pageSize, IEnumerable<Guid> categoriesId, bool? isDocumentation, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ArticleModel>> GetPopularPaginatedWithoutBlocksArticlesAsync(int pageNo, int pageSize, bool? isDocumentation, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ArticleModel>> GetPaginatedByAuthorWithoutBlocksArticlesAsync(Guid authorId, int pageNo, int pageSize, CancellationToken cancellationToken = default);

    public Task<long> CountAsync(bool? isDocumentation, CancellationToken cancellationToken = default);

    public Task<long> CountByCategoryAsync(IEnumerable<Guid> categoriesId, bool? isDocumentation, CancellationToken cancellationToken = default);

    public Task<long> CountByAuthorAsync(Guid authorId,  CancellationToken cancellationToken = default);

    public Task<ArticleModel?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task CreateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default);

    public Task UpdateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default);

    public Task DeleteArticleAsync(ArticleModel article, CancellationToken cancellationToken = default);
}
