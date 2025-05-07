using LedgerLite.SharedKernel.Events;

namespace LedgerLite.Users.Contracts.IntegrationEvents;

public sealed record OrganizationCreatedIntegrationEvent(string Name) : IEvent;