using LedgerLite.Accounting.Core.Domain.Periods;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed class FiscalPeriodRepository(AccountingDbContext context) : IFiscalPeriodRepository
{
    public void Add(FiscalPeriod period) => context.FiscalPeriods.Add(period);

    public Task<FiscalPeriod?> GetByIdAsync(Guid id, CancellationToken token) =>
        context.FiscalPeriods.FirstOrDefaultAsync(x => x.Id == id, token);
}