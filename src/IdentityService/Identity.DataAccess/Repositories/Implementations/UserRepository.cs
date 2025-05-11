using Identity.Domain.Entities;
using Identity.Domain.Repositories.Abstractions;
using Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly DbSet<User> _users;

    public UserRepository(ApplicationDbContext context)
    {
        _users = context.Users;
    }

    public Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _users
            .Include(u => u.Followings).ThenInclude(f => f.Author)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<User?> GetUserByNameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _users
            .Include(u => u.Followings).ThenInclude(f => f.Author)
            .FirstOrDefaultAsync(u => u.NormalizedUserName!.Equals(username, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
    }

    public async Task<IEnumerable<User>> GetPaginatedUsersByPartialUsernameAsync(int pageNo, int pageSize, string partialUsername, CancellationToken cancellationToken = default)
    {
        return await _users
            .AsNoTracking()
            .Where(x => x.NormalizedUserName!.Contains(partialUsername, StringComparison.CurrentCultureIgnoreCase))
            .Union(
                _users
                .AsNoTracking()
                .Where(x => x.PublicUsername!.Contains(partialUsername, StringComparison.CurrentCultureIgnoreCase))
            ).Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }


    public async Task<long> GetCountByPartialNameAsync(string partialUsername, CancellationToken cancellationToken = default)
    {
        return await _users
            .AsNoTracking()
            .Where(x => x.NormalizedUserName!.Contains(partialUsername, StringComparison.CurrentCultureIgnoreCase))
            .Union(
                _users
                .AsNoTracking()
                .Where(x => x.PublicUsername!.Contains(partialUsername, StringComparison.CurrentCultureIgnoreCase))
            ).CountAsync(cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellation = default)
    {
        _users.Update(user);

        return Task.CompletedTask;
    }
}
