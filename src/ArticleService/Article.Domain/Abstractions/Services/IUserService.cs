namespace Article.Domain.Abstractions.Services;

public interface IUserService
{
    public Task<bool> IsUserExist(string username, CancellationToken cancellationToken = default);
}
