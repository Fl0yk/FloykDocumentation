using Article.Domain.Abstractions.Repositories;
using Article.Infrastructure.Shared.Models;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace Article.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Lazy<IArticleRepository> _articleRepository;
    private readonly Lazy<ICategoryRepository> _categoryRepository;

    public IArticleRepository ArticleRepository => _articleRepository.Value;

    public ICategoryRepository CatergoryRepository => _categoryRepository.Value;

    public UnitOfWork(IMongoCollection<ArticleDb> articles, IMongoCollection<CategoryDb> categories, IDistributedCache cach, IMapper mapper)
    {
        var baseCategoryRep = new CategoryRepository(categories, articles, mapper);

        _articleRepository = new(() => new ArticleRepository(articles, categories, mapper));
        _categoryRepository = new(() => new CachCategoryRepository(baseCategoryRep, cach));
    }
}
