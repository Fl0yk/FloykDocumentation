using Article.Domain.Abstractions.Repositories;
using Article.Domain.Entities;
using Article.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Article.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly DbSet<User> _users;

    public UserRepository(SqlDbContext context)
    {
        _users = context.Users;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _users
            .AsNoTracking()
            .Where(x => x.Username == username)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Update(user);
        
        return Task.CompletedTask;
    }

    public Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Add(user);

        return Task.CompletedTask;
    }
}
