using Ardalis.Result;

namespace LedgerLite.Accounting.Core.Domain.Periods;

public static class FiscalPeriodErrors
{
    private const string FiscalPeriodIdentifier = "FiscalPeriod";

    public static ValidationError StartIsAfterEnd(DateOnly start, DateOnly end) =>
        new(identifier: FiscalPeriodIdentifier,
            errorMessage: $"Period's start date ({start:O}) is after the specified end date ({end:O})",
            errorCode: "FP-START_AFTER_END",
            severity: ValidationSeverity.Error);
}