using Core.Infrastructure.DataBase;

namespace Forum.Infrastructure.Data;

internal sealed class TransactionProvider : BaseTransactionProvider<ApplicationDbContext>
{
    public TransactionProvider(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
