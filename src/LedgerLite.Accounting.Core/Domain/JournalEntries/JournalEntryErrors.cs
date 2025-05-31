using Ardalis.Result;
using Humanizer;
using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

internal static class JournalEntryErrors
{
    private const string LineIdentifier = "JournalEntryLine";
    private const string EntryIdentifier = "JournalEntry";

    public static ValidationError MoreThanTwoLinesWhenTypeIsNotCompound(int lineCount)
    {
        return new ValidationError(
            LineIdentifier,
            $"Only 2 journal entry lines are allowed when the journal entry type is not {JournalEntryType.Compound}. (Got {lineCount} lines)",
            "JEN.LINES-OVERFLOW",
            ValidationSeverity.Error);
    }

    public static ValidationError LessThanTwoLines(int lineCount)
    {
        return new ValidationError(
            LineIdentifier,
            $"Got {lineCount} journal entry lines, expected a minimum amount of 2 lines.",
            "JEN.LINE-UNDERFLOW",
            ValidationSeverity.Error);
    }

    public static ValidationError SameTransactionTypeOnBothLines(TransactionType type)
    {
        return new ValidationError(
            LineIdentifier,
            $"Got 2 journal entry lines of type {type}, expected one {TransactionType.Credit} line and one {TransactionType.Debit}",
            "JEN.LINE-SAME_TRANSACTIONS",
            ValidationSeverity.Error);
    }

    public static ValidationError EmptyReferenceNumber()
    {
        return new ValidationError(
            EntryIdentifier,
            "A journal entry requires a non-empty reference number.",
            "JEN-EMPTY_REF_NUMBER",
            ValidationSeverity.Error);
    }

    public static ValidationError AlreadyPosted()
    {
        return new ValidationError(
            EntryIdentifier,
            "The journal entry has already been posted.",
            "JEN-ALREADY_POSTED",
            ValidationSeverity.Error);
    }

    public static ValidationError CannotEdit(JournalEntryStatus status)
    {
        return new ValidationError(
            EntryIdentifier,
            $"Cannot edit the journal entry because it is {status}.",
            "JEN-NO_EDIT",
            ValidationSeverity.Error);
    }

    public static ValidationError CantPostBecauseIsReversed()
    {
        return new ValidationError(
            EntryIdentifier,
            "The journal entry has been reversed and can't be posted",
            "JEN-NO_POST_REVERSED",
            ValidationSeverity.Error);
    }

    public static ValidationError Imbalanced()
    {
        return new ValidationError(
            EntryIdentifier,
            "The journal entry is imbalanced.",
            "JEN-IMBALANCED",
            ValidationSeverity.Error);
    }

    public static ValidationError CannotEditBecausePeriodIsClosed(FiscalPeriod period)
    {
        if (!period.ClosedAtUtc.HasValue)
            throw new InvalidOperationException(
                "Tried to generate error for closed fiscal period, but period is not closed.");

        var timeSinceClose = DateTime.UtcNow - period.ClosedAtUtc.Value;

        return new ValidationError(
            EntryIdentifier,
            $"Cannot post to specified fiscal period because it was closed {timeSinceClose.Humanize()} ago.",
            "JEN-PERIOD_CLOSED",
            ValidationSeverity.Error);
    }
}