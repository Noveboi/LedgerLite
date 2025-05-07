namespace LedgerLite.SharedKernel.Events;

internal sealed class SequentialEventPublisher : IEventPublisher
{
    public async ValueTask PublishAsync(IEnumerable<EventExecutor> eventExecutors, IEvent e, CancellationToken token)
    {
        foreach (var executor in eventExecutors)
        {
            await executor.Callback(e, token);
        }
    }
}