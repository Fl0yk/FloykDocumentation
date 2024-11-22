﻿using Identity.DataAccess.Data;
using Identity.DataAccess.Entities;
using Identity.DataAccess.Repositories.Abstractions;
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
            .Include(u => u.SavedArticles)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<User?> GetUserByNameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _users
            .Include(u => u.Followings).ThenInclude(f => f.Author)
            .Include(u => u.SavedArticles)
            .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper(), cancellationToken);
    }

    public Task<bool> IsUserExist(Guid id, CancellationToken cancellationToken = default)
    {
        return _users.Where(u => u.Id == id).AnyAsync(cancellationToken);
    }

    public Task<bool> IsUserExist(string username, CancellationToken cancellationToken = default)
    {
        return _users.Where(u => u.NormalizedUserName == username.ToUpper()).AnyAsync(cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellation = default)
    {
        _users.Update(user);

        return Task.CompletedTask;
    }
}
