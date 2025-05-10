using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

public interface IChartOfAccountsRepository
{
    void Add(ChartOfAccounts chart);
    Task<ChartOfAccounts?> GetByIdAsync(Guid id, CancellationToken token);
    Task<ChartOfAccounts?> GetByOrganizationIdAsync(Guid organizationId, CancellationToken token);
}