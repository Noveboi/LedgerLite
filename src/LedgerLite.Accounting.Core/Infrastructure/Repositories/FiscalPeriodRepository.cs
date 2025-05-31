using LedgerLite.Accounting.Core.Domain.Periods;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed class FiscalPeriodRepository(AccountingDbContext context) : IFiscalPeriodRepository
{
    public void Add(FiscalPeriod period)
    {
        context.FiscalPeriods.Add(period);
    }

    public Task<FiscalPeriod?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return context.FiscalPeriods.FirstOrDefaultAsync(x => x.Id == id, token);
    }

    public Task<bool> NameExistsForOrganizationAsync(Guid organizationId, string name, CancellationToken token)
    {
        return context.FiscalPeriods.AnyAsync(x => x.OrganizationId == organizationId && x.Name == name, token);
    }

    public async Task<IReadOnlyList<FiscalPeriod>> GetForOrganizationAsync(Guid organizationId, CancellationToken token)
    {
        return await context.FiscalPeriods.Where(x => x.OrganizationId == organizationId).ToListAsync(token);
    }

    public Task<FiscalPeriod?> FindOverlappingPeriodAsync(Guid organizationId, DateOnly startDate, DateOnly endDate,
        CancellationToken token)
    {
        return context.FiscalPeriods
            .Where(x => x.OrganizationId == organizationId)
            .Where(x => (x.EndDate > startDate && x.EndDate < endDate) ||
                        (x.StartDate > startDate && x.StartDate < endDate))
            .FirstOrDefaultAsync(token);
    }
}