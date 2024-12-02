namespace Forum.Domain.Abstractions.Services;
public interface IUserService
{
    public Task<bool> IsUserExist(Guid id, CancellationToken cancellationToken = default);
}
