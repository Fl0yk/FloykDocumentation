namespace Article.Domain.Abstractions.Repositories;

public interface IUnitOfWork
{
    public IArticleRepository ArticleRepository { get; }

    public ICatergoryRepository CatergoryRepository { get; }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
