using LedgerLite.Accounting.Core.Domain.Chart;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed class ChartOfAccountsRepository(AccountingDbContext context) : IChartOfAccountsRepository
{
    public void Add(ChartOfAccounts chart)
    {
        context.Charts.Add(entity: chart);
    }

    public Task<ChartOfAccounts?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return context.Charts.FirstOrDefaultAsync(predicate: x => x.Id == id, cancellationToken: token);
    }

    public Task<ChartOfAccounts?> GetByOrganizationIdAsync(Guid organizationId, CancellationToken token)
    {
        return context.Charts
            .Include(navigationPropertyPath: x => x.Nodes)
            .ThenInclude(navigationPropertyPath: x => x.Account)
            .FirstOrDefaultAsync(predicate: x => x.OrganizationId == organizationId, cancellationToken: token);
    }
}