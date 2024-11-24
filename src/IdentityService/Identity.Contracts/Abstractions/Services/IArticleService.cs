using Identity.DataAccess.Entities;

namespace Identity.Contracts.Abstractions.Services;

public interface IArticleService
{
    public Task<bool> IsArticleExistAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<Article> GetArticleByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
