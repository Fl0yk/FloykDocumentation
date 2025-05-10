using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Forum.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly DbSet<User> _users;

    public UserRepository(ApplicationDbContext context)
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

    public Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Add(user);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Update(user);

        return Task.CompletedTask;
    }
}
