namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;

internal sealed record FiscalPeriodDto(
    Guid Id,
    DateOnly StartDate,
    DateOnly EndDate,
    DateTime? ClosedAtUtc);