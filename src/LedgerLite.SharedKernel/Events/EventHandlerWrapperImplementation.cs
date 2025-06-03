using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.SharedKernel.Events;

internal sealed class EventHandlerWrapperImplementation<TEvent> : EventHandlerWrapper where TEvent : IEvent
{
    public override ValueTask PropagateEventAsync(IEvent e, IServiceProvider sp,
        Func<IEnumerable<EventExecutor>, IEvent, CancellationToken, ValueTask> publish, CancellationToken token)
    {
        var handlers = sp
            .GetServices<IEventHandler<TEvent>>()
            .Select(static x => new EventExecutor(HandlerInstance: x,
                (ev, tok) => x.HandleAsync((TEvent)ev, token: tok)));

        return publish(arg1: handlers, arg2: e, arg3: token);
    }
}