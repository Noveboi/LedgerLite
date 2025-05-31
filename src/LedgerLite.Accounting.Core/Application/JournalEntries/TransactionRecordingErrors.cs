using Ardalis.Result;

namespace LedgerLite.Accounting.Core.Application.JournalEntries;

public static class TransactionRecordingErrors
{
    public const string TransactionIdentifier = "TransactionRecording";

    public static ValidationError FiscalPeriodNotFound(Guid periodId)
    {
        return new ValidationError(
            identifier: TransactionIdentifier,
            errorMessage: $"Fiscal period '{periodId}' does not exist.",
            errorCode: "TRA-FP_NOT_FOUND",
            severity: ValidationSeverity.Error);
    }
}