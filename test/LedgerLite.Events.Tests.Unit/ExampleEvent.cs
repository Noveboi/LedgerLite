using LedgerLite.SharedKernel.Events;

namespace LedgerLite.Events.Tests.Unit;

public sealed record ExampleEvent(string Message) : IEvent;