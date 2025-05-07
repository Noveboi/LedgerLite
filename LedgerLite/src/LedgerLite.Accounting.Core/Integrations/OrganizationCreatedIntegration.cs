using LedgerLite.SharedKernel.Events;
using LedgerLite.Users.Contracts.IntegrationEvents;
using Serilog;

namespace LedgerLite.Accounting.Core.Integrations;

public sealed class OrganizationCreatedIntegration : IEventHandler<OrganizationCreatedIntegrationEvent>
{
    public ValueTask HandleAsync(OrganizationCreatedIntegrationEvent e, CancellationToken token)
    {
        Log.Information("Accounting module received integration event {@event}", e);
        return ValueTask.CompletedTask;
    }
}