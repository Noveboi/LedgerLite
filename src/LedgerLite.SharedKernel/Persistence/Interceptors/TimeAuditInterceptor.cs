using LedgerLite.SharedKernel.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace LedgerLite.SharedKernel.Persistence.Interceptors;

internal sealed class TimeAuditInterceptor : SaveChangesInterceptor
{
    private readonly ILogger _log = Log.ForContext<TimeAuditInterceptor>();

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null)
            return await base.SavingChangesAsync(eventData: eventData, result: result,
                cancellationToken: cancellationToken);

        var utcNow = DateTime.UtcNow;
        var entries = eventData.Context.ChangeTracker.Entries<IAuditable>().ToList();

        var createCount = 0;
        var updateCount = 0;

        foreach (var entry in entries)
            switch (entry.State)
            {
                case EntityState.Added:
                    createCount++;
                    entry.Entity.CreatedAtUtc = utcNow;
                    break;
                case EntityState.Modified:
                    updateCount++;
                    entry.Entity.ModifiedAtUtc = utcNow;
                    break;
            }

        LogAuditCount(verb: "CREATED", count: createCount);
        LogAuditCount(verb: "UPDATED", count: updateCount);

        return await base.SavingChangesAsync(eventData: eventData, result: result,
            cancellationToken: cancellationToken);
    }

    private void LogAuditCount(string verb, int count)
    {
        if (count == 0)
            return;

        _log.Information(messageTemplate: "Audit {count} entities {verb}", propertyValue0: count, propertyValue1: verb);
    }
}