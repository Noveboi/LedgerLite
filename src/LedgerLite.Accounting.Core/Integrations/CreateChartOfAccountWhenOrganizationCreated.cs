using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.SharedKernel.Events;
using LedgerLite.Users.Contracts.IntegrationEvents;
using Serilog;

namespace LedgerLite.Accounting.Core.Integrations;

internal sealed class CreateChartOfAccountWhenOrganizationCreated(IAccountingUnitOfWork unitOfWork)
    : IEventHandler<OrganizationCreatedIntegrationEvent>
{
    private readonly ILogger _log = Log.ForContext<CreateChartOfAccountWhenOrganizationCreated>();

    public async ValueTask HandleAsync(OrganizationCreatedIntegrationEvent e, CancellationToken token)
    {
        _log.Information("Creating new '{chartName}' because an organization was created.", nameof(ChartOfAccounts));

        var result = await ChartOfAccounts.Create(e.Id)
            .Bind(chart =>
            {
                unitOfWork.ChartOfAccountsRepository.Add(chart);
                return Result.Success(chart);
            })
            .BindAsync(chart => unitOfWork.SaveChangesAsync(token).MapAsync(() => chart));

        if (!result.IsSuccess)
        {
            _log.Error("Could not create {chartName}. Error: {@error}", nameof(ChartOfAccounts), result);
            return;
        }

        _log.Information("Created {chartName} ({chartId})  for organization '{orgName}'",
            nameof(ChartOfAccounts),
            e.Name,
            result.Value.Id);
    }
}