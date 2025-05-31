using LedgerLite.SharedKernel.Domain.Events;
using LedgerLite.SharedKernel.Events;
using Serilog;

namespace LedgerLite.SharedKernel;

public abstract class IntegrationEventPropagator<TDomainEvent, TIntegrationEvent>(IPublisher publisher)
    : IEventHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
    where TIntegrationEvent : IEvent
{
    private readonly ILogger _log = Log.ForContext<IntegrationEventPropagator<TDomainEvent, TIntegrationEvent>>();

    public ValueTask HandleAsync(TDomainEvent e, CancellationToken token)
    {
        var integrationEvent = MapToIntegrationEvent(domainEvent: e);
        _log.Information(messageTemplate: "Propagating integration event {@event}", propertyValue: integrationEvent);

        return publisher.PublishAsync(e: integrationEvent, token: token);
    }

    protected abstract TIntegrationEvent MapToIntegrationEvent(TDomainEvent domainEvent);
}