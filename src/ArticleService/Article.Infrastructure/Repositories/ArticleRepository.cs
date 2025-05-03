using Article.Domain.Abstractions.Repositories;
using Article.Infrastructure.Shared.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

using ArticleModel = Article.Domain.Entities.Article;

namespace Article.Infrastructure.Repositories;

//TODO: помучать чат для оптимизации запросов с работой блоков
public class ArticleRepository : IArticleRepository
{
    private readonly IMongoCollection<ArticleDb> _articles;
    private readonly IMapper _mapper;

    public ArticleRepository(
        IMongoCollection<ArticleDb> articles, 
        IMapper mapper)
    {
        _articles = articles;
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

        if (dbArticle is null)
        {
            return null;
        }

        return _mapper.Map<ArticleModel>(dbArticle);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> isPublishedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsPublished, true);

        return await _articles.CountDocumentsAsync(isPublishedFilter, cancellationToken: cancellationToken);
    }

    public async Task<long> CountByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> categoryFilter = Builders<ArticleDb>.Filter.Eq(article => article.CategoryId, categoryId);

        return await _articles.CountDocumentsAsync(categoryFilter, cancellationToken: cancellationToken);
    }

    public async Task<long> CountByAuthorAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> authorFilter = Builders<ArticleDb>.Filter.Eq(article => article.AuthorId, authorId);

        return await _articles.CountDocumentsAsync(authorFilter, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ArticleModel>> GetPaginatedByAuthorWithoutBlocksArticlesAsync(Guid authorId, int pageNo, int pageSize, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> authorFilter = Builders<ArticleDb>.Filter.Eq(article => article.AuthorId, authorId);
        ProjectionDefinition<ArticleDb, ArticleDb> shortProjection = Builders<ArticleDb>.Projection.Exclude(article => article.Blocks);

        var dbArticles = await _articles
                            .Find(authorFilter)
                            .Project(shortProjection)
                            .Skip((pageNo - 1) * pageSize)
                            .Limit(pageSize)
                            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
    }

    public async Task<IEnumerable<ArticleModel>> GetPaginatedByCategoryWithoutBlocksArticlesAsync(Guid categoryId, int pageNo, int pageSize, CancellationToken cancellationToken = default)
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

        return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
    }

    public async Task<IEnumerable<ArticleModel>> GetPaginatedByDateWithoutBlocksArticlesAsync(int pageNo, int pageSize, CancellationToken cancellationToken = default)
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

        return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
    }

    public Task UpdateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default)
    {
        var dbArticle = _mapper.Map<ArticleDb>(article);

        FilterDefinition<ArticleDb> idFilter = Builders<ArticleDb>.Filter.Eq(a => a.Id, article.Id);
        UpdateDefinition<ArticleDb> updateDefinition = Builders<ArticleDb>.Update
                                                                            .Set(a => a.Title, dbArticle.Title)
                                                                            .Set(a => a.AuthorId, dbArticle.AuthorId)
                                                                            .Set(a => a.CategoryId, dbArticle.CategoryId)
                                                                            .Set(a => a.Blocks, dbArticle.Blocks)
                                                                            .Set(a => a.IsPublished, dbArticle.IsPublished)
                                                                            .Set(a => a.DateOfPublication, dbArticle.DateOfPublication);

        return _articles.UpdateOneAsync(idFilter, updateDefinition, cancellationToken: cancellationToken);
    }
}
