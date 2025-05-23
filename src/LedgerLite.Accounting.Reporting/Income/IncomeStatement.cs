using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Reporting.Income;

internal sealed class IncomeStatement : AuditableEntity
{
    public FiscalPeriod Period { get; private set; } = null!;
}