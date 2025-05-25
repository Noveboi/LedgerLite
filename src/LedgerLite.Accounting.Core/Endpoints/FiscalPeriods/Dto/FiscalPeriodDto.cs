using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;

public sealed record FiscalPeriodDto(
    Guid Id,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTime? ClosedAtUtc,
    string Name)
{
    public static FiscalPeriodDto FromEntity(FiscalPeriod fiscalPeriod) => new(
        Id: fiscalPeriod.Id,
        StartDate: fiscalPeriod.StartDate,
        EndDate: fiscalPeriod.EndDate,
        ClosedAtUtc: fiscalPeriod.ClosedAtUtc,
        Name: fiscalPeriod.Name);
}