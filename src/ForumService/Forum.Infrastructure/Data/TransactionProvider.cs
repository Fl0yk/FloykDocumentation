using Core.Infrastructure.DataBase;
using Forum.Infrastructure.Database;

namespace Forum.Infrastructure.Data;

internal sealed class TransactionProvider : BaseTransactionProvider<ApplicationDbContext>
{
    public TransactionProvider(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
