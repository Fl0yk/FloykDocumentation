using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Article.Infrastructure.Data;
using Article.Infrastructure.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Article.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly DbSet<Category> _categories;
    private readonly IMongoCollection<ArticleDb> _articles;

    public CategoryRepository(
        SqlDbContext dbContext, 
        IMongoCollection<ArticleDb> articles)
    {
        _categories = dbContext.Categories;
        _articles = articles;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _categories.AsNoTracking().ToArrayAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsExistArticleInCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> categoryFilter = Builders<ArticleDb>.Filter.Eq(article => article.CategoryId, categoryId);

        long count = await _articles.CountDocumentsAsync(categoryFilter, cancellationToken: cancellationToken);

        return count > 0;
    }
}
