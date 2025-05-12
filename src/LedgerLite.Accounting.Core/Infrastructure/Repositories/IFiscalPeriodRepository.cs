using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

public interface IFiscalPeriodRepository
{
    void Add(FiscalPeriod period);
    Task<FiscalPeriod?> GetByIdAsync(Guid id, CancellationToken token);

    Task<FiscalPeriod?> FindOverlappingPeriodAsync(Guid organizationId, DateOnly startDate, DateOnly endDate, CancellationToken token);
}