using Ardalis.Result;
using LedgerLite.SharedKernel.Models;

namespace LedgerLite.Accounting.Core.Domain.Periods;

public static class FiscalPeriodErrors
{
    private const string FiscalPeriodIdentifier = "FiscalPeriod";

    public static ValidationError StartIsAfterEnd(DateOnly start, DateOnly end)
    {
        return new ValidationError(FiscalPeriodIdentifier,
            $"Period's start date ({start:O}) is after the specified end date ({end:O})",
            "FP-START_AFTER_END",
            ValidationSeverity.Error);
    }

    public static ValidationError OverlappingPeriods(DateRange a, DateRange b)
    {
        var overlap = a.GetOverlapWith(b);
        if (!overlap.HasValue)
            throw new InvalidOperationException("Date ranges do not overlap.");

        return new ValidationError(FiscalPeriodIdentifier,
            $"Periods overlap from {overlap.Value.Start:O} to {overlap.Value.End:O}",
            "FP-OVERLAP",
            ValidationSeverity.Error);
    }

    public static ValidationError NameCannotBeEmpty()
    {
        return new ValidationError(FiscalPeriodIdentifier,
            "A period must have a non-empty name.",
            "FP-NAME_EMPTY",
            ValidationSeverity.Error);
    }

    public static ValidationError PeriodWithSameName(string name)
    {
        return new ValidationError(FiscalPeriodIdentifier,
            $"A fiscal period named '{name}' already exists.",
            "FP-NAME_ALREADY_EXISTS",
            ValidationSeverity.Error);
    }
}