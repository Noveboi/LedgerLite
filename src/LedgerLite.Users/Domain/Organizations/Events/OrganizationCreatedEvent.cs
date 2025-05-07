using LedgerLite.SharedKernel.Domain.Events;

namespace LedgerLite.Users.Domain.Organizations.Events;

internal sealed class OrganizationCreatedEvent(string name) : DomainEvent
{
    public string Name { get; } = name;
}