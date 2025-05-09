using LedgerLite.SharedKernel.Events;

namespace LedgerLite.Users.Contracts.IntegrationEvents;

public sealed record OrganizationCreatedIntegrationEvent(Guid Id, string Name) : IEvent;