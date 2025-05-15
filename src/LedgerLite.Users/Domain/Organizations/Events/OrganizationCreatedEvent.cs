using LedgerLite.SharedKernel.Domain.Events;

namespace LedgerLite.Users.Domain.Organizations.Events;

internal sealed class OrganizationCreatedEvent(Organization org) : DomainEvent
{
    public Organization Organization { get; } = org;
}