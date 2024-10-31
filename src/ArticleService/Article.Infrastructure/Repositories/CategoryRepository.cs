using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Article.Infrastructure.Shared.Models;
using AutoMapper;
using MongoDB.Driver;

namespace Article.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<CategoryDb> _categories;
    private readonly IMapper _mapper;

    public CategoryRepository(IMongoCollection<CategoryDb> categories, IMapper mapper)
    {
        _categories = categories;
        _mapper = mapper;
    }

    public Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        var dbCategory = _mapper.Map<CategoryDb>(category);

        return _categories.InsertOneAsync(dbCategory, cancellationToken: cancellationToken);
    }

    public Task DelteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<CategoryDb> idFilter = Builders<CategoryDb>.Filter.Eq(c => c.Id, id);

        return _categories.DeleteOneAsync(idFilter, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categories.Find("{}").ToListAsync(cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<Category>>(categories);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<CategoryDb> idFilter = Builders<CategoryDb>.Filter.Eq(c => c.Id, id);

        var category = await _categories.Find(idFilter).FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map<Category>(category);
    }
}
