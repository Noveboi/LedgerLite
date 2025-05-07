namespace LedgerLite.SharedKernel.Events;

internal interface IEventPublisher
{
    ValueTask PublishAsync(IEnumerable<EventExecutor> eventExecutors, IEvent e, CancellationToken token);
}