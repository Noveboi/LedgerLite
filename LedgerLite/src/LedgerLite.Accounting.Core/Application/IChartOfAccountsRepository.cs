using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Application;

public interface IChartOfAccountsRepository
{
    void Add(ChartOfAccounts chart);
    Task<ChartOfAccounts?> GetByIdAsync(Guid id, CancellationToken token);
}