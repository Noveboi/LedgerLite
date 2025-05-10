using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;

namespace LedgerLite.Accounting.Core.Application.Chart;

public sealed class ChartOfAccountsService(IChartOfAccountsRepository repository) : IChartOfAccountsService
{
    public async Task<Result<ChartOfAccounts>> GetByOrganizationIdAsync(Guid? organizationId, CancellationToken token)
    {
        if (!organizationId.HasValue)
        {
            return Result.NotFound("User does not belong in an organization.");
        }

        if (await repository.GetByOrganizationIdAsync(organizationId.Value, token) is not { } chart)
        {
            return Result.NotFound($"Organization with ID '{organizationId.Value}' does not exist.");
        }

        return chart;
    }
}