using Core.Infrastructure.DataBase;
using Identity.Infrastructure.Database;

namespace Identity.Infrastructure.Data;

public sealed class TransactionProvider : BaseTransactionProvider<ApplicationDbContext>
{
    public TransactionProvider(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
