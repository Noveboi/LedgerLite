using LedgerLite.SharedKernel.Domain;
using LedgerLite.SharedKernel.Domain.Events;
using LedgerLite.SharedKernel.Events;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace LedgerLite.SharedKernel.Persistence.Interceptors;

internal sealed class DomainEventInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    private readonly ILogger _log = Log.ForContext<DomainEventInterceptor>();
    private readonly List<DomainEvent> _unprocessedEvents = [];

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not { } context)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var domainEvents = context.ChangeTracker
            .Entries<IHaveDomainEvents>()
            .Where(x => x.Entity.DomainEvents.Count > 0)
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        if (domainEvents.Count > 0)
        {
            _log.Information("Queuing {eventCount} domain events for processing", domainEvents.Count);
            _unprocessedEvents.AddRange(domainEvents);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken token = new())
    {
        if (_unprocessedEvents.Count > 0)
        {
            _log.Information("Processing and publishing {eventData} domain events...", _unprocessedEvents.Count);
            foreach (var domainEvent in _unprocessedEvents) await publisher.PublishAsync(domainEvent, token);

            _unprocessedEvents.Clear();
        }

        return await base.SavedChangesAsync(eventData, result, token);
    }
}