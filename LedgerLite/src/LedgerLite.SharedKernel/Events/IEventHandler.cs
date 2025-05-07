namespace LedgerLite.SharedKernel.Events;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    ValueTask HandleAsync(TEvent e, CancellationToken token);
}