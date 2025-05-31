using LedgerLite.Accounting.Core.Domain.Chart;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed class ChartOfAccountsRepository(AccountingDbContext context) : IChartOfAccountsRepository
{
    public void Add(ChartOfAccounts chart)
    {
        context.Charts.Add(chart);
    }

    public Task<ChartOfAccounts?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return context.Charts.FirstOrDefaultAsync(x => x.Id == id, token);
    }

    public Task<ChartOfAccounts?> GetByOrganizationIdAsync(Guid organizationId, CancellationToken token)
    {
        return context.Charts
            .Include(x => x.Nodes)
            .ThenInclude(x => x.Account)
            .FirstOrDefaultAsync(x => x.OrganizationId == organizationId, token);
    }
}