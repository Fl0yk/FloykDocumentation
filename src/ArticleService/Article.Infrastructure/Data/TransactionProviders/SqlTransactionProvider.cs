using Core.Infrastructure.DataBase;

namespace Article.Infrastructure.Data.TransactionProviders;

internal sealed class SqlTransactionProvider : BaseTransactionProvider<SqlDbContext>
{
    public SqlTransactionProvider(SqlDbContext dbContext) : base(dbContext)
    {
    }
}
