using Identity.Domain.Entities;

namespace Identity.Domain.Repositories.Abstractions;

public interface IUserRepository
{
    public Task UpdateAsync(User user, CancellationToken cancellation = default);

    public Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<User?> GetUserByNameAsync(string username, CancellationToken cancellationToken = default);

    Task<IEnumerable<User>> GetPaginatedUsersByPartialUsernameAsync(int pageNo, int pageSize, string partialUsername, CancellationToken cancellationToken = default);

    Task<long> GetCountByPartialNameAsync(string partialUsername, CancellationToken cancellationToken = default);
}
