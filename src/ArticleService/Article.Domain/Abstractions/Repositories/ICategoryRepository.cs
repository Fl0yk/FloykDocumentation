using Article.Domain.Entities;

namespace Article.Domain.Abstractions.Repositories;

public interface ICategoryRepository
{
    public Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);

    public Task<Category?> GetCategoryByIdAsync(Guid id,  CancellationToken cancellationToken = default);

    public Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default);

    public Task DelteCategoryAsync(Guid id, CancellationToken cancellationToken = default);
}
