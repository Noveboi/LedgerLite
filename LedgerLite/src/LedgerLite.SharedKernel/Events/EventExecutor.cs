namespace LedgerLite.SharedKernel.Events;

internal sealed record EventExecutor(object HandlerInstance, Func<IEvent, CancellationToken, ValueTask> Callback);