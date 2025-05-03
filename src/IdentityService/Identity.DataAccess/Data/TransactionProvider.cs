using Core.Infrastructure.DataBase;
using Identity.DataAccess.Data;

namespace Identity.Infrastructure.Data;

public sealed class TransactionProvider : BaseTransactionProvider<ApplicationDbContext>
{
    public TransactionProvider(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
