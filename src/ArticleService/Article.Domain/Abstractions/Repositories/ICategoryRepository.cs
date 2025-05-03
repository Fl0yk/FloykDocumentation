using Article.Domain.Entities;

namespace Article.Domain.Abstractions.Repositories;

public interface ICategoryRepository
{
    public Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);

    public Task<Category?> GetCategoryByIdAsync(Guid id,  CancellationToken cancellationToken = default);

    public Task<bool> IsExistArticleInCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
