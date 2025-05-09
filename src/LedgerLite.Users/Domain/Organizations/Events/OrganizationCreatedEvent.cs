using LedgerLite.SharedKernel.Domain.Events;

namespace LedgerLite.Users.Domain.Organizations.Events;

internal sealed class OrganizationCreatedEvent(Guid id, string name) : DomainEvent
{
    public Guid Id { get; } = id; 
    public string Name { get; } = name;
}