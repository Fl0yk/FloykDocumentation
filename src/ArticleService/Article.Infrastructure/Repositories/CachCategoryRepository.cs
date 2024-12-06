using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Article.Infrastructure.Repositories;

public class CachCategoryRepository : ICategoryRepository
{
    private readonly CategoryRepository _baseRepository;
    private readonly IDistributedCache _cache;
    private const string _categoriesKey = "categories-key";

    public CachCategoryRepository(CategoryRepository baseRepository, IDistributedCache cache)
    {
        _baseRepository = baseRepository;
        _cache = cache;
    }

    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        string? categories = await _cache.GetStringAsync(_categoriesKey, cancellationToken);

        if (categories is not null)
        {
            await _cache.RemoveAsync(_categoriesKey, cancellationToken);
        }

        await _baseRepository.AddCategoryAsync(category, cancellationToken);
    }

    public async Task DelteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string key = $"categoryId-{id}";

        string? value = await _cache.GetStringAsync(key, cancellationToken);

        if (value is not null)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }

        string? categories = await _cache.GetStringAsync(_categoriesKey, cancellationToken);

        if (value is not null)
        {
            await _cache.RemoveAsync(_categoriesKey, cancellationToken);
        }

        await _baseRepository.DelteCategoryAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        string? value = await _cache.GetStringAsync(_categoriesKey, cancellationToken);

        if (value is not null)
        {
            return JsonConvert.DeserializeObject<IEnumerable<Category>>(value)!;
        }

        return await _baseRepository.GetAllCategoriesAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string key = $"categoryId-{id}";

        string? value = await _cache.GetStringAsync(key, cancellationToken);

        if (value is not null)
        {
            return JsonConvert.DeserializeObject<Category>(value);
        }

        var category = await _baseRepository.GetCategoryByIdAsync(id, cancellationToken);

        if (category is not null)
        {
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(category), cancellationToken);
        }

        return category;
    }

    public Task<bool> IsExistArticleInCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return _baseRepository.IsExistArticleInCategoryAsync(categoryId, cancellationToken);
    }
}
