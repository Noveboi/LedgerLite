using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Infrastructure.Persistence;

public interface IChartOfAccountsRepository
{
    Task<ChartOfAccounts?> GetByIdAsync(Guid id, CancellationToken token);
}