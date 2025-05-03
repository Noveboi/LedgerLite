using LedgerLite.Accounting.Domain.Accounts;
using LedgerLite.Accounting.Domain.Chart;

namespace LedgerLite.Accounting.Infrastructure.Persistence;

public interface IChartOfAccountsRepository
{
    Task<ChartOfAccounts?> GetByIdAsync(Guid id, CancellationToken token);
}