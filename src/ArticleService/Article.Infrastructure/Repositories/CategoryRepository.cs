using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Article.Infrastructure.Shared.Models;
using AutoMapper;
using MongoDB.Driver;
using System.Threading;

namespace Article.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<CategoryDb> _categories;
    private readonly IMongoCollection<ArticleDb> _articles;
    private readonly IMapper _mapper;

    public CategoryRepository(IMongoCollection<CategoryDb> categories, IMongoCollection<ArticleDb> articles, IMapper mapper)
    {
        _categories = categories;
        _articles = articles;
        _mapper = mapper;
    }

    public Task AddCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        var dbCategory = _mapper.Map<CategoryDb>(category);

        return _categories.InsertOneAsync(dbCategory, cancellationToken: cancellationToken);
    }

    public Task DelteCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return RecursiveDeleteAsync(id, cancellationToken);
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

    public async Task<bool> IsExistArticleInCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> categoryFilter = Builders<ArticleDb>.Filter.Eq(article => article.CategoryId, categoryId);

        long count = await _articles.CountDocumentsAsync(categoryFilter, cancellationToken: cancellationToken);

        return count > 0;
    }

    private async Task RecursiveDeleteAsync(Guid parentId, CancellationToken cancellationToken)
    {
        FilterDefinition<CategoryDb> idParentFilter = Builders<CategoryDb>.Filter.Eq(c => c.ParentId, parentId);
        FilterDefinition<CategoryDb> idFilter = Builders<CategoryDb>.Filter.Eq(c => c.Id, parentId);

        var children = await _categories.Find(idParentFilter).ToListAsync(cancellationToken);

        foreach (var child in children)
        {
            await RecursiveDeleteAsync(child.Id, cancellationToken);
        }

        await _categories.DeleteOneAsync(idFilter, cancellationToken: cancellationToken);
    }
}
