using Article.Domain.Entities;

namespace Article.Domain.Abstractions.Repositories;

public interface ICatergoryRepository
{
    public Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);

    public Task<Category?> GetCategoryById(Guid id,  CancellationToken cancellationToken = default);

    public Task<Category?> GetCategoryWhithArticlesByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task AddCategoryAsync(Category category, Category parent, CancellationToken cancellationToken = default);

    public Task DelteCategoryAsync(Guid id, CancellationToken cancellationToken = default);
}
