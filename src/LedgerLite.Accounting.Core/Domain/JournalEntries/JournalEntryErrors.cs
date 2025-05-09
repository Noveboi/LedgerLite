using Ardalis.Result;
using Humanizer;
using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

internal static class JournalEntryErrors
{
    private const string LineIdentifier = "JournalEntryLine";
    private const string EntryIdentifier = "JournalEntry";
    
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

    public static ValidationError EmptyReferenceNumber() => new(
        identifier: EntryIdentifier,
        errorMessage: "A journal entry requires a non-empty reference number.",
        errorCode: "JEN-EMPTY_REF_NUMBER",
        severity: ValidationSeverity.Error);

    public static ValidationError AlreadyPosted() => new(
        identifier: EntryIdentifier,
        errorMessage: "The journal entry has already been posted.",
        errorCode: "JEN-ALREADY_POSTED",
        severity: ValidationSeverity.Error);

    public static ValidationError CannotEdit(JournalEntryStatus status) => new(
        identifier: EntryIdentifier,
        errorMessage: $"Cannot edit the journal entry because it is {status}.",
        errorCode: "JEN-NO_EDIT",
        severity: ValidationSeverity.Error);

    public static ValidationError CantPostBecauseIsReversed() => new(
        identifier: EntryIdentifier,
        errorMessage: "The journal entry has been reversed and can't be posted",
        errorCode: "JEN-NO_POST_REVERSED",
        severity: ValidationSeverity.Error);

    public static ValidationError Imbalanced() => new(
        identifier: EntryIdentifier,
        errorMessage: "The journal entry is imbalanced.",
        errorCode: "JEN-IMBALANCED",
        severity: ValidationSeverity.Error);
    
    public static ValidationError CannotEditBecausePeriodIsClosed(FiscalPeriod period)
    {
        if (!period.ClosedAtUtc.HasValue)
            throw new InvalidOperationException("Tried to generate error for closed fiscal period, but period is not closed.");

        var timeSinceClose = DateTime.UtcNow - period.ClosedAtUtc.Value;
        
        return new ValidationError(
            identifier: EntryIdentifier,
            errorMessage: $"Cannot post to specified fiscal period because it was closed {timeSinceClose.Humanize()} ago.",
            errorCode: "JEN-PERIOD_CLOSED",
            severity: ValidationSeverity.Error);
    }
}