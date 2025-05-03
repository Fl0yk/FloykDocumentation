namespace Identity.Domain.Repositories.Abstractions;

public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
