using LedgerLite.Accounting.Core.Application;
using LedgerLite.Accounting.Core.Domain.Chart;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure;

internal sealed class ChartOfAccountsRepository(AccountingDbContext context) : IChartOfAccountsRepository
{
    public void Add(ChartOfAccounts chart) => context.Charts.Add(chart);

    public Task<ChartOfAccounts?> GetByIdAsync(Guid id, CancellationToken token) =>
        context.Charts.FirstOrDefaultAsync(x => x.Id == id, token);
}