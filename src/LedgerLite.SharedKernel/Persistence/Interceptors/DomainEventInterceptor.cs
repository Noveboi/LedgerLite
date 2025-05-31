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
            return await base.SavingChangesAsync(eventData: eventData, result: result,
                cancellationToken: cancellationToken);

        var domainEvents = context.ChangeTracker
            .Entries<IHaveDomainEvents>()
            .Where(predicate: x => x.Entity.DomainEvents.Count > 0)
            .SelectMany(selector: x => x.Entity.DomainEvents)
            .ToList();

        if (domainEvents.Count > 0)
        {
            _log.Information(messageTemplate: "Queuing {eventCount} domain events for processing",
                propertyValue: domainEvents.Count);
            _unprocessedEvents.AddRange(collection: domainEvents);
        }

        return await base.SavingChangesAsync(eventData: eventData, result: result,
            cancellationToken: cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken token = new())
    {
        if (_unprocessedEvents.Count > 0)
        {
            _log.Information(messageTemplate: "Processing and publishing {eventData} domain events...",
                propertyValue: _unprocessedEvents.Count);
            foreach (var domainEvent in _unprocessedEvents) await publisher.PublishAsync(e: domainEvent, token: token);

            _unprocessedEvents.Clear();
        }

        return await base.SavedChangesAsync(eventData: eventData, result: result, cancellationToken: token);
    }
}