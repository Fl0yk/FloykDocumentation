using Core.Providers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Core.Infrastructure.DataBase;
public abstract class BaseTransactionProvider<TContext> : ITransactionProvider, IDisposable
        where TContext : DbContext
{
    private readonly TContext _dbContext;

    private IDbContextTransaction? _transaction;
    private bool _disposedValue;

    protected BaseTransactionProvider(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task OpenTransaction(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            throw new Exception("Transaction already opened");
        }

        _transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
    }

    public Task Commit(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            return _transaction.CommitAsync(cancellationToken);
        }

        return Task.CompletedTask;
    }

    public Task Rollback(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            return _transaction.RollbackAsync(cancellationToken);
        }

        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _transaction = null;
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    ~BaseTransactionProvider()
    {
        Dispose(disposing: false);
    }
}
