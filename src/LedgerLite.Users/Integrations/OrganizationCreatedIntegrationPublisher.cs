using LedgerLite.SharedKernel;
using LedgerLite.SharedKernel.Events;
using LedgerLite.Users.Contracts.IntegrationEvents;
using LedgerLite.Users.Domain.Organizations.Events;

namespace LedgerLite.Users.Integrations;

internal sealed class OrganizationCreatedIntegrationPublisher(IPublisher publisher) :
    IntegrationEventPropagator<OrganizationCreatedEvent, OrganizationCreatedIntegrationEvent>(publisher: publisher)
{
    protected override OrganizationCreatedIntegrationEvent MapToIntegrationEvent(OrganizationCreatedEvent domainEvent)
    {
        return new OrganizationCreatedIntegrationEvent(Id: domainEvent.Organization.Id,
            Name: domainEvent.Organization.Name);
    }
}