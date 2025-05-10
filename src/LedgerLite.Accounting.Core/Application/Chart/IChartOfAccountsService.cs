using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Core.Application.Chart;

public interface IChartOfAccountsService
{
    Task<Result<ChartOfAccounts>> GetByUserIdAsync(Guid userId, CancellationToken token);
}