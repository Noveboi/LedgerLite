using LedgerLite.SharedKernel;
using LedgerLite.SharedKernel.Events;
using LedgerLite.Users.Contracts.IntegrationEvents;
using LedgerLite.Users.Domain.Organizations.Events;

namespace LedgerLite.Users.Integrations;


internal sealed class OrganizationCreatedIntegrationPublisher(IPublisher publisher) : 
    IntegrationEventPropagator<OrganizationCreatedEvent, OrganizationCreatedIntegrationEvent>(publisher)
{
    protected override OrganizationCreatedIntegrationEvent MapToIntegrationEvent(OrganizationCreatedEvent domainEvent) => 
        new(domainEvent.Organization.Id, domainEvent.Organization.Name);
}