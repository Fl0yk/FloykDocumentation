using Identity.Domain.Repositories.Abstractions;
using Identity.Infrastructure.Database;

namespace Identity.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IUserRepository> _userRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _userRepository = new(() => new UserRepository(_context));
    }

    public IUserRepository UserRepository => _userRepository.Value;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
