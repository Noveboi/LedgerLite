using FastEndpoints;

namespace LedgerLite.SharedKernel.Domain.Events;

public abstract class DomainEvent : IEvent
{
    public DateTime OccuredAtUtc { get; } = DateTime.UtcNow;
}