using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Article.Infrastructure.Shared.Models;
using MongoDB.Driver;

namespace Article.Infrastructure.Repositories;

public class CategoryRepository : ICatergoryRepository
{
    private readonly IMongoCollection<CategoryDb> _categories;

    public CategoryRepository(IMongoCollection<CategoryDb> categories)
    {
        _categories = categories;
    }

    public Task AddCategoryAsync(Category category, Category parent, CancellationToken cancellationToken = default)
    {
        
        throw new NotImplementedException();
    }

    public Task DelteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Category?> GetCategoryById(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Category?> GetCategoryWhithArticlesByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
