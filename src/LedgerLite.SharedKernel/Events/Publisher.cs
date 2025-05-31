using System.Collections.Concurrent;

namespace LedgerLite.SharedKernel.Events;

internal sealed class Publisher(IEventPublisher eventPublisher, IServiceProvider serviceProvider) : IPublisher
{
    private static readonly ConcurrentDictionary<Type, EventHandlerWrapper> EventHandlers = [];

    public ValueTask PublishAsync<TEvent>(TEvent e, CancellationToken token) where TEvent : IEvent
    {
        var handler = EventHandlers.GetOrAdd(key: e.GetType(), valueFactory: static eventType =>
        {
            var wrapperType = typeof(EventHandlerWrapperImplementation<>).MakeGenericType(eventType);
            var wrapper = Activator.CreateInstance(type: wrapperType) ??
                          throw new InvalidOperationException(message: $"Couldn't create wrapper for type {eventType}");
            return (EventHandlerWrapper)wrapper;
        });

        return handler.PropagateEventAsync(e: e, sp: serviceProvider, publish: PublishCoreAsync, token: token);
    }

    private ValueTask PublishCoreAsync(
        IEnumerable<EventExecutor> eventExecutors,
        IEvent e,
        CancellationToken token)
    {
        return eventPublisher.PublishAsync(eventExecutors: eventExecutors, e: e, token: token);
    }
}