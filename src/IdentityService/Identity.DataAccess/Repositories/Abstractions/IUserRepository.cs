using Identity.DataAccess.Entities;

namespace Identity.DataAccess.Repositories.Abstractions;

public interface IUserRepository
{
    public Task UpdateAsync(User user, CancellationToken cancellation = default);

    public Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<User?> GetUserByNameAsync(string username, CancellationToken cancellationToken = default);

    public Task<bool> IsUserExist(Guid id, CancellationToken cancellationToken = default);

    public Task<bool> IsUserExist(string username, CancellationToken cancellationToken = default);
}
