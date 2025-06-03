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
        _log.Information(messageTemplate: "Creating new '{chartName}' because an organization was created.",
            nameof(ChartOfAccounts));

        var result = await ChartOfAccounts.Create(organizationId: e.Id)
            .Bind(chart =>
            {
                unitOfWork.ChartOfAccountsRepository.Add(chart: chart);
                return Result.Success(value: chart);
            })
            .BindAsync(chart => unitOfWork.SaveChangesAsync(token: token).MapAsync(() => chart));

        if (!result.IsSuccess)
        {
            _log.Error(messageTemplate: "Could not create {chartName}. Error: {@error}",
                nameof(ChartOfAccounts), propertyValue1: result);
            return;
        }

        _log.Information(messageTemplate: "Created {chartName} ({chartId})  for organization '{orgName}'",
            nameof(ChartOfAccounts),
            propertyValue1: e.Name,
            propertyValue2: result.Value.Id);
    }
}