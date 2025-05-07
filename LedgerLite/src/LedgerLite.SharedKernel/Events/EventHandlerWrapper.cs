namespace LedgerLite.SharedKernel.Events;

internal abstract class EventHandlerWrapper
{
    public abstract ValueTask PropagateEventAsync(
        IEvent e,
        IServiceProvider sp,
        Func<IEnumerable<EventExecutor>, IEvent, CancellationToken, ValueTask> publish,
        CancellationToken token);
}