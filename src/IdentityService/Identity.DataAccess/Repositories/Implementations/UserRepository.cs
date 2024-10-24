using Identity.DataAccess.Data;
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

}
