using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Infrastructure;

public interface IChartOfAccountsRepository
{
    Task<ChartOfAccounts?> GetByIdAsync(Guid id, CancellationToken token);
}