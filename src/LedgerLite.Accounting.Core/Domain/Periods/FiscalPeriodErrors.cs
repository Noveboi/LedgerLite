using Ardalis.Result;
using LedgerLite.SharedKernel.Models;

namespace LedgerLite.Accounting.Core.Domain.Periods;

public static class FiscalPeriodErrors
{
    private const string FiscalPeriodIdentifier = "FiscalPeriod";

    public static ValidationError StartIsAfterEnd(DateOnly start, DateOnly end)
    {
        return new ValidationError(identifier: FiscalPeriodIdentifier,
            errorMessage: $"Period's start date ({start:O}) is after the specified end date ({end:O})",
            errorCode: "FP-START_AFTER_END",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError OverlappingPeriods(DateRange a, DateRange b)
    {
        var overlap = a.GetOverlapWith(other: b);
        if (!overlap.HasValue)
            throw new InvalidOperationException(message: "Date ranges do not overlap.");

        return new ValidationError(identifier: FiscalPeriodIdentifier,
            errorMessage: $"Periods overlap from {overlap.Value.Start:O} to {overlap.Value.End:O}",
            errorCode: "FP-OVERLAP",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError NameCannotBeEmpty()
    {
        return new ValidationError(identifier: FiscalPeriodIdentifier,
            errorMessage: "A period must have a non-empty name.",
            errorCode: "FP-NAME_EMPTY",
            severity: ValidationSeverity.Error);
    }

    public static ValidationError PeriodWithSameName(string name)
    {
        return new ValidationError(identifier: FiscalPeriodIdentifier,
            errorMessage: $"A fiscal period named '{name}' already exists.",
            errorCode: "FP-NAME_ALREADY_EXISTS",
            severity: ValidationSeverity.Error);
    }
}