namespace LedgerLite.SharedKernel.Events;

public interface IPublisher
{
    ValueTask PublishAsync<TEvent>(TEvent e, CancellationToken token) where TEvent : IEvent;
}