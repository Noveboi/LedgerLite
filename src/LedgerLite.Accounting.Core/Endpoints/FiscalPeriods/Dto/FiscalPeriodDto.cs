using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;

public sealed record FiscalPeriodDto(
    Guid Id,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTime? ClosedAtUtc,
    string Name)
{
    public static FiscalPeriodDto FromEntity(FiscalPeriod fiscalPeriod)
    {
        return new FiscalPeriodDto(
            fiscalPeriod.Id,
            fiscalPeriod.StartDate,
            fiscalPeriod.EndDate,
            fiscalPeriod.ClosedAtUtc,
            fiscalPeriod.Name);
    }
}