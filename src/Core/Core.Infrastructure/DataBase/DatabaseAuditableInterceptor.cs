using Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace Core.Infrastructure.DataBase;

//TODO: проверить, что работает
public sealed class DatabaseAuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        FillAuditColumns(eventData);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        FillAuditColumns(eventData);
        return base.SavingChanges(eventData, result);
    }

    private void FillAuditColumns(DbContextEventData eventData)
    {
        var nowUtc = DateTimeOffset.UtcNow;

        foreach (var entry in eventData.Context!.ChangeTracker.Entries())
        {
            if (entry is { Entity: IBaseEntity auditableEntity })
            {
                if (entry is { State: EntityState.Added })
                {
                    auditableEntity.CreatedAt = nowUtc;
                }

                if (entry is { State: EntityState.Modified })
                {
                    auditableEntity.UpdatedAt = nowUtc;
                }

                if (entry is { State: EntityState.Deleted })
                {
                    entry.State = EntityState.Modified;

                    auditableEntity.IsDeleted = true;
                    auditableEntity.DeletedAt = nowUtc;
                }
            }
        }
    }
}
