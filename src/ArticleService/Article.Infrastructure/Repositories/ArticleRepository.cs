using Article.Domain.Abstractions.Repositories;
using Article.Infrastructure.Shared.Models;
using AutoMapper;
using MongoDB.Driver;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Infrastructure.Repositories;

public class ArticleRepository :  IArticleRepository
{
    private readonly IMongoCollection<ArticleDb> _articles;
    private readonly IMongoCollection<CategoryDb> _categories;
    private readonly IMapper _mapper;

    public ArticleRepository(IMongoCollection<ArticleDb> articles, IMongoCollection<CategoryDb> categories, IMapper mapper)
    {
        _articles = articles;
        _categories = categories;
        _mapper = mapper;
    }

    public async Task CreateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default)
    {
        var dbArticle = _mapper.Map<ArticleDb>(article);

        await _articles.InsertOneAsync(dbArticle, cancellationToken: cancellationToken);
    }

    public Task DeleteArticleAsync(ArticleModel article, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> idFilter = Builders<ArticleDb>.Filter.Eq(article => article.Id, article.Id);

        return _articles.DeleteOneAsync(idFilter, cancellationToken: cancellationToken);
    }

    public async Task<ArticleModel?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> idFilter = Builders<ArticleDb>.Filter.Eq(article => article.Id, id);

        var dbArticle = await _articles.Find(idFilter).FirstOrDefaultAsync(cancellationToken);

        FilterDefinition<CategoryDb> idCategoryFilter = Builders<CategoryDb>.Filter.Eq(c => c.Id, dbArticle.CategoryId);

        dbArticle.Category = _categories.Find(idCategoryFilter).First();

        return _mapper.Map<ArticleModel>(dbArticle);
    }

    public async Task<IEnumerable<ArticleModel>> GetPaginatedByCategoryWithoutBlocksArticles(Guid categoryId, int pageNo, int pageSize, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> categoryFilter = Builders<ArticleDb>.Filter.Eq(article => article.CategoryId, categoryId);
        FilterDefinition<ArticleDb> isPublishedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsPublished, true);
        ProjectionDefinition<ArticleDb, ArticleDb> shortProjection = Builders<ArticleDb>.Projection.Exclude(article => article.Blocks);

        var dbArticles = await _articles
                            .Find(categoryFilter & isPublishedFilter)
                            .Project(shortProjection)
                            .Skip((pageNo - 1) * pageSize)
                            .Limit(pageSize)
                            .ToListAsync(cancellationToken);

        foreach (var dbArticle in dbArticles)
        {
            FilterDefinition<CategoryDb> idCategoryFilter = Builders<CategoryDb>.Filter.Eq(c => c.Id, dbArticle.CategoryId);

            dbArticle.Category = _categories.Find(idCategoryFilter).First();
        }

        return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
    }

    public async Task<IEnumerable<ArticleModel>> GetPaginatedByDateWithoutBlocksArticles(int pageNo, int pageSize, CancellationToken cancellationToken = default)
    {
        SortDefinition<ArticleDb> sortByDateDefinition = Builders<ArticleDb>.Sort.Ascending(article => article.DateOfPublication);
        FilterDefinition<ArticleDb> isPublishedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsPublished, true);
        ProjectionDefinition<ArticleDb, ArticleDb> shortProjection = Builders<ArticleDb>.Projection.Exclude(article => article.Blocks);

        var dbArticles = await _articles
                        .Find(isPublishedFilter)
                        .Project(shortProjection)
                        .Sort(sortByDateDefinition)
                        .Skip((pageNo - 1) * pageSize)
                        .Limit(pageSize)
                        .ToListAsync(cancellationToken);

        foreach (var dbArticle in dbArticles)
        {
            FilterDefinition<CategoryDb> idCategoryFilter = Builders<CategoryDb>.Filter.Eq(c => c.Id, dbArticle.CategoryId);

            dbArticle.Category = _categories.Find(idCategoryFilter).First();
        }

        return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
    }

    public Task UpdateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default)
    {
        var dbArticle = _mapper.Map<ArticleDb>(article);

        FilterDefinition<ArticleDb> idFilter = Builders<ArticleDb>.Filter.Eq(article => article.Id, article.Id);
        UpdateDefinition<ArticleDb> updateDefinition = Builders<ArticleDb>.Update
                                                                            .Set(a => a.Title, dbArticle.Title)
                                                                            .Set(a => a.CategoryId, dbArticle.CategoryId)
                                                                            .Set(a => a.Blocks, dbArticle.Blocks);

        return _articles.UpdateOneAsync(idFilter, updateDefinition, cancellationToken: cancellationToken);
    }
}
