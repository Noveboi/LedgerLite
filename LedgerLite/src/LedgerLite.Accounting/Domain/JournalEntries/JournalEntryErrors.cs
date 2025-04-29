using Ardalis.Result;

namespace LedgerLite.Accounting.Domain.JournalEntries;

internal static class JournalEntryErrors
{
    private const string LineIdentifier = "JournalEntryLine";
    
    public static ValidationError MoreThanTwoLinesWhenTypeIsNotCompound(int lineCount) => new(
        identifier: LineIdentifier,
        errorMessage: $"Only 2 journal entry lines are allowed when the journal entry type is not {JournalEntryType.Compound}. (Got {lineCount} lines)",
        errorCode: "JEN.LINES-OVERFLOW",
        severity: ValidationSeverity.Error);

    public static ValidationError LessThanTwoLines(int lineCount) => new(
        identifier: LineIdentifier,
        errorMessage: $"Got {lineCount} journal entry lines, expected a minimum amount of 2 lines.",
        errorCode: "JEN.LINE-UNDERFLOW",
        severity: ValidationSeverity.Error);

    public static ValidationError SameTransactionTypeOnBothLines(TransactionType type) => new(
        identifier: LineIdentifier,
        errorMessage: $"Got 2 journal entry lines of type {type}, expected one {TransactionType.Credit} line and one {TransactionType.Debit}",
        errorCode: "JEN.LINE-SAME_TRANSACTIONS",
        severity: ValidationSeverity.Error);
}