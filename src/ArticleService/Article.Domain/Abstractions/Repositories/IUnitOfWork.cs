namespace Article.Domain.Abstractions.Repositories;

public interface IUnitOfWork
{
    public IArticleRepository ArticleRepository { get; }

    public ICategoryRepository CatergoryRepository { get; }
}
