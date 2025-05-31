using System.Collections.Concurrent;

namespace LedgerLite.SharedKernel.Events;

internal sealed class Publisher(IEventPublisher eventPublisher, IServiceProvider serviceProvider) : IPublisher
{
    private static readonly ConcurrentDictionary<Type, EventHandlerWrapper> EventHandlers = [];

    public ValueTask PublishAsync<TEvent>(TEvent e, CancellationToken token) where TEvent : IEvent
    {
        var handler = EventHandlers.GetOrAdd(e.GetType(), static eventType =>
        {
            var wrapperType = typeof(EventHandlerWrapperImplementation<>).MakeGenericType(eventType);
            var wrapper = Activator.CreateInstance(wrapperType) ??
                          throw new InvalidOperationException($"Couldn't create wrapper for type {eventType}");
            return (EventHandlerWrapper)wrapper;
        });

        return handler.PropagateEventAsync(e, serviceProvider, PublishCoreAsync, token);
    }

    private ValueTask PublishCoreAsync(
        IEnumerable<EventExecutor> eventExecutors,
        IEvent e,
        CancellationToken token)
    {
        return eventPublisher.PublishAsync(eventExecutors, e, token);
    }
}