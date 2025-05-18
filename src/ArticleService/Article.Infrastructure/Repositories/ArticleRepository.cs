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

        UpdateDefinition<ArticleDb> updateDefinition = Builders<ArticleDb>.Update
                                                                            .Set(a => a.IsDeleted, true)
                                                                            .Set(a => a.DeletedAt, DateTimeOffset.UtcNow);

        return _articles.UpdateOneAsync(idFilter, updateDefinition, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ArticleModel>> GetShouldBeApprovedArticlesAsync(CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> shulBeApprovedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsShouldBeApproved, true);
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false);
        ProjectionDefinition<ArticleDb, ArticleDb> shortProjection = Builders<ArticleDb>.Projection.Exclude(article => article.Blocks);
        SortDefinition<ArticleDb> sortDefinition = Builders<ArticleDb>.Sort.Ascending(article => article.CreatedAt);

        var dbArticles = await _articles
                                .Find(isDeletedFilter & shulBeApprovedFilter)
                                .Project(shortProjection)
                                .Sort(sortDefinition)
                                .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
    }

    public async Task<ArticleModel?> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> idFilter = Builders<ArticleDb>.Filter.Eq(article => article.Id, id);
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false);

        var dbArticle = await _articles.Find(isDeletedFilter & idFilter).FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map<ArticleModel>(dbArticle);
    }

    public async Task<long> CountAsync(bool? isDocumentation, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false);
        FilterDefinition<ArticleDb> isPublishedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsPublished, true);

        if (isDocumentation is null)
        {
            return await _articles.CountDocumentsAsync(isDeletedFilter & isPublishedFilter, cancellationToken: cancellationToken);
        }
        else
        {
            FilterDefinition<ArticleDb> isDocumentFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDocumentation, isDocumentation);

            return await _articles.CountDocumentsAsync(isDeletedFilter & isPublishedFilter & isDocumentFilter, cancellationToken: cancellationToken);
        }
    }

    public async Task<long> CountByCategoryAsync(IEnumerable<Guid> categoriesId, bool? isDocumentation, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false); 
        FilterDefinition<ArticleDb> categoriesFilter = Builders<ArticleDb>.Filter.In(a => a.CategoryId, categoriesId);
        FilterDefinition<ArticleDb> isPublishedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsPublished, true);

        if (isDocumentation is null)
        {
            return await _articles.CountDocumentsAsync(isDeletedFilter & isPublishedFilter & categoriesFilter, cancellationToken: cancellationToken);
        }
        else
        {
            FilterDefinition<ArticleDb> isDocumentFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDocumentation, isDocumentation);

            return await _articles.CountDocumentsAsync(isDeletedFilter & isPublishedFilter & isDocumentFilter & categoriesFilter, cancellationToken: cancellationToken);
        }
    }

    public async Task<long> CountByAuthorAsync(Guid authorId, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false);
        FilterDefinition<ArticleDb> authorFilter = Builders<ArticleDb>.Filter.Eq(article => article.AuthorId, authorId);

        return await _articles.CountDocumentsAsync(isDeletedFilter & authorFilter, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ArticleModel>> GetPaginatedByAuthorWithoutBlocksArticlesAsync(Guid authorId, int pageNo, int pageSize, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> authorFilter = Builders<ArticleDb>.Filter.Eq(article => article.AuthorId, authorId);
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false);
        ProjectionDefinition<ArticleDb, ArticleDb> shortProjection = Builders<ArticleDb>.Projection.Exclude(article => article.Blocks);
        SortDefinition<ArticleDb> sortDefinition = Builders<ArticleDb>.Sort.Descending(article => article.CreatedAt);

        var dbArticles = await _articles
                                .Find(isDeletedFilter & authorFilter)
                                .Project(shortProjection)
                                .Sort(sortDefinition)
                                .Skip((pageNo - 1) * pageSize)
                                .Limit(pageSize)
                                .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
    }

    public async Task<IEnumerable<ArticleModel>> GetPopularPaginatedWithoutBlocksArticlesAsync(int pageNo, int pageSize, bool? isDocumentation, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false);
        FilterDefinition<ArticleDb> isPublishedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsPublished, true);
        ProjectionDefinition<ArticleDb, ArticleDb> shortProjection = Builders<ArticleDb>.Projection.Exclude(article => article.Blocks);
        SortDefinition<ArticleDb> sortDefinition = Builders<ArticleDb>.Sort.Descending(article => article.VisitCount);

        if (isDocumentation is null)
        {
            var dbArticles = await _articles
                                .Find(isDeletedFilter & isPublishedFilter)
                                .Project(shortProjection)
                                .Sort(sortDefinition)
                                .Skip((pageNo - 1) * pageSize)
                                .Limit(pageSize)
                                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
        }
        else
        {
            FilterDefinition<ArticleDb> isDocumentFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDocumentation, isDocumentation);
            var dbArticles = await _articles
                                .Find(isDeletedFilter & isPublishedFilter & isDocumentFilter)
                                .Project(shortProjection)
                                .Sort(sortDefinition)
                                .Skip((pageNo - 1) * pageSize)
                                .Limit(pageSize)
                                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
        }
    }

    public async Task<IEnumerable<ArticleModel>> GetPopularPaginatedWithoutBlocksArticlesAsync(int pageNo, int pageSize, IEnumerable<Guid> categories, bool? isDocumentation, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false);
        FilterDefinition<ArticleDb> isPublishedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsPublished, true);
        FilterDefinition<ArticleDb> categoriesFilter = Builders<ArticleDb>.Filter.In(a => a.CategoryId, categories);
        SortDefinition<ArticleDb> sortDefinition = Builders<ArticleDb>.Sort.Descending(article => article.VisitCount);
        ProjectionDefinition<ArticleDb, ArticleDb> shortProjection = Builders<ArticleDb>.Projection.Exclude(article => article.Blocks);

        if (isDocumentation is null)
        {
            var dbArticles = await _articles
                                .Find(isDeletedFilter & categoriesFilter & isPublishedFilter)
                                .Project(shortProjection)
                                .Sort(sortDefinition)
                                .Skip((pageNo - 1) * pageSize)
                                .Limit(pageSize)
                                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
        }
        else
        {
            FilterDefinition<ArticleDb> isDocumentFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDocumentation, isDocumentation);

            var dbArticles = await _articles
                                .Find(isDeletedFilter & categoriesFilter & isPublishedFilter & isDocumentFilter)
                                .Project(shortProjection)
                                .Sort(sortDefinition)
                                .Skip((pageNo - 1) * pageSize)
                                .Limit(pageSize)
                                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
        }
    }

    public async Task<IEnumerable<ArticleModel>> GetPaginatedByDateWithoutBlocksArticlesAsync(int pageNo, int pageSize, bool? isDocumentation, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false);
        SortDefinition<ArticleDb> sortByDateDefinition = Builders<ArticleDb>.Sort.Descending(article => article.DateOfPublication);
        FilterDefinition<ArticleDb> isPublishedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsPublished, true);
        ProjectionDefinition<ArticleDb, ArticleDb> shortProjection = Builders<ArticleDb>.Projection.Exclude(article => article.Blocks);

        if (isDocumentation is null)
        { 
            var dbArticles = await _articles
                            .Find(isDeletedFilter & isPublishedFilter)
                            .Project(shortProjection)
                            .Sort(sortByDateDefinition)
                            .Skip((pageNo - 1) * pageSize)
                            .Limit(pageSize)
                            .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
        }
        else
        {
            FilterDefinition<ArticleDb> isDocumentFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDocumentation, isDocumentation);

            var dbArticles = await _articles
                        .Find(isDeletedFilter & isPublishedFilter & isDocumentFilter)
                        .Project(shortProjection)
                        .Sort(sortByDateDefinition)
                        .Skip((pageNo - 1) * pageSize)
                        .Limit(pageSize)
                        .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
        }
    }

    public async Task<IEnumerable<ArticleModel>> GetPaginatedByDateWithoutBlocksArticlesAsync(int pageNo, int pageSize, IEnumerable<Guid> categoriesId, bool? isDocumentation, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> isDeletedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDeleted, false);
        SortDefinition<ArticleDb> sortByDateDefinition = Builders<ArticleDb>.Sort.Descending(article => article.DateOfPublication);
        FilterDefinition<ArticleDb> isPublishedFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsPublished, true);
        FilterDefinition<ArticleDb> categoriesFilter = Builders<ArticleDb>.Filter.In(a => a.CategoryId, categoriesId);
        ProjectionDefinition<ArticleDb, ArticleDb> shortProjection = Builders<ArticleDb>.Projection.Exclude(article => article.Blocks);

        if (isDocumentation is null)
        {
            var dbArticles = await _articles
                            .Find(isDeletedFilter & categoriesFilter & isPublishedFilter)
                            .Project(shortProjection)
                            .Sort(sortByDateDefinition)
                            .Skip((pageNo - 1) * pageSize)
                            .Limit(pageSize)
                            .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
        }
        else
        {
            FilterDefinition<ArticleDb> isDocumentFilter = Builders<ArticleDb>.Filter.Eq(article => article.IsDocumentation, isDocumentation);

            var dbArticles = await _articles
                            .Find(isDeletedFilter & categoriesFilter & isPublishedFilter & isDocumentFilter)
                            .Project(shortProjection)
                            .Sort(sortByDateDefinition)
                            .Skip((pageNo - 1) * pageSize)
                            .Limit(pageSize)
                            .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<ArticleModel>>(dbArticles);
        }
    }

    public Task UpdateArticleAsync(ArticleModel article, CancellationToken cancellationToken = default)
    {
        var dbArticle = _mapper.Map<ArticleDb>(article);

        FilterDefinition<ArticleDb> idFilter = Builders<ArticleDb>.Filter.Eq(a => a.Id, article.Id);
        UpdateDefinition<ArticleDb> updateDefinition = Builders<ArticleDb>.Update
                                                                            .Set(a => a.Title, dbArticle.Title)
                                                                            .Set(a => a.AuthorId, dbArticle.AuthorId)
                                                                            .Set(a => a.CategoryId, dbArticle.CategoryId)
                                                                            .Set(a => a.VisitCount, dbArticle.VisitCount)
                                                                            .Set(a => a.IsDocumentation, dbArticle.IsDocumentation)
                                                                            .Set(a => a.Blocks, dbArticle.Blocks)
                                                                            .Set(a => a.IsPublished, dbArticle.IsPublished)
                                                                            .Set(a => a.IsShouldBeApproved, dbArticle.IsShouldBeApproved)
                                                                            .Set(a => a.ShortDescription, dbArticle.ShortDescription)
                                                                            .Set(a => a.DateOfPublication, dbArticle.DateOfPublication)
                                                                            .Set(a => a.UpdatedAt, DateTimeOffset.UtcNow);

        return _articles.UpdateOneAsync(idFilter, updateDefinition, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<ArticleModel>> GetArticlesByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        FilterDefinition<ArticleDb> idsFilter = Builders<ArticleDb>.Filter.In(a => a.Id, ids);

        var dbArticle = await _articles.Find(idsFilter).ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ArticleModel>>(dbArticle);
    }
}
