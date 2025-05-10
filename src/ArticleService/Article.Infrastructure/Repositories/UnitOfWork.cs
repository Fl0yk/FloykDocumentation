using Article.Domain.Abstractions.Repositories;
using Article.Infrastructure.Database;
using Article.Infrastructure.Shared.Models;
using AutoMapper;
using MongoDB.Driver;

namespace Article.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Lazy<IArticleRepository> _articleRepository;
    private readonly Lazy<ICategoryRepository> _categoryRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly SqlDbContext _dbContext;

    public IArticleRepository ArticleRepository => _articleRepository.Value;

    public ICategoryRepository CatergoryRepository => _categoryRepository.Value;

    public IUserRepository UserRepository => _userRepository.Value;

    public UnitOfWork(IMongoCollection<ArticleDb> articles, SqlDbContext dbContext, IMapper mapper)
    {
        _articleRepository = new(() => new ArticleRepository(articles, mapper));
        _categoryRepository = new(() => new CategoryRepository(dbContext, articles));
        _userRepository = new(() => new UserRepository(dbContext));
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
