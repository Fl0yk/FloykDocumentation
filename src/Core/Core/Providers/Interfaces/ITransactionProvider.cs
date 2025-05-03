namespace Core.Providers.Interfaces;

public interface ITransactionProvider
{
    Task OpenTransaction(CancellationToken cancellationToken);
    Task Commit(CancellationToken cancellationToken);
}
