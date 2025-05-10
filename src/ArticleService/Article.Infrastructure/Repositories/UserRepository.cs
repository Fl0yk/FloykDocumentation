using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Article.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Article.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly SqlDbContext _context;

    public UserRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        
        return Task.CompletedTask;
    }

    public Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(user);

        return Task.CompletedTask;
    }

    public Task SaveArticle(SavedArticle savedArticle, CancellationToken cancellationToken = default)
    {
        _context.SavedArticles.Add(savedArticle);

        return Task.CompletedTask;
    }

    public Task UnsaveArticle(SavedArticle savedArticle, CancellationToken cancellationToken = default)
    {
        _context.SavedArticles.Remove(savedArticle);

        return Task.CompletedTask;
    }

    public async Task<User?> WithSavedArticles(User? user, CancellationToken cancellationToken = default)
    {
        if (user is null)
        {
            return null!;
        }

        user.SavedArticles = await _context.SavedArticles.Where(sa => sa.UserId == user.Id).ToListAsync(cancellationToken);

        return user;
    }

    public Task RemoveSavedArticlesByArticleAsync(Guid articleId, CancellationToken cancellationToken = default)
    {
        var savedArticles = _context.SavedArticles
            .AsNoTracking()
            .Where(x => x.ArticleId == articleId);

        _context.SavedArticles.RemoveRange(savedArticles);

        return Task.CompletedTask;
    }
}
