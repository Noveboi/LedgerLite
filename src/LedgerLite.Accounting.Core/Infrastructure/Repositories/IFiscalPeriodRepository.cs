using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

public interface IFiscalPeriodRepository
{
    void Add(FiscalPeriod period);
    Task<FiscalPeriod?> GetByIdAsync(Guid id, CancellationToken token);
}