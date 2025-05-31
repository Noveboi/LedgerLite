using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

/// <summary>
///     Records one financial transaction.
/// </summary>
public sealed class JournalEntry : AuditableEntity
{
    private readonly List<JournalEntryLine> _lines = [];

    private JournalEntry()
    {
    }

    public Guid FiscalPeriodId { get; private init; }
    public Guid CreatedByUserId { get; private init; }
    public Guid? LastModifiedByUserId { get; private init; }

    public string ReferenceNumber { get; private init; } = null!;
    public DateOnly OccursAt { get; private init; }
    public string Description { get; private init; } = null!;
    public JournalEntryType Type { get; private init; } = null!;
    public JournalEntryStatus Status { get; private set; } = null!;
    public IReadOnlyCollection<JournalEntryLine> Lines => _lines;

    /// <summary>
    ///     Start recording a new journal entry.
    /// </summary>
    public static Result<JournalEntry> Create(
        FiscalPeriod fiscalPeriod,
        JournalEntryType type,
        string referenceNumber,
        string description,
        DateOnly occursAt,
        Guid createdByUserId)
    {
        if (string.IsNullOrWhiteSpace(value: referenceNumber))
            return Result.Invalid(validationError: JournalEntryErrors.EmptyReferenceNumber());

        if (fiscalPeriod.IsClosed)
            return Result.Invalid(
                validationError: JournalEntryErrors.CannotEditBecausePeriodIsClosed(period: fiscalPeriod));

        return Result.Success(value: new JournalEntry
        {
            Type = type,
            ReferenceNumber = referenceNumber,
            Description = description,
            OccursAt = occursAt,
            Status = JournalEntryStatus.Editable,
            CreatedByUserId = createdByUserId,
            FiscalPeriodId = fiscalPeriod.Id
        });
    }

    public bool IsBalanced()
    {
        var debitAmount = _lines
            .Where(predicate: x => x.TransactionType == TransactionType.Debit)
            .Sum(selector: x => x.Amount);

        var creditAmount = _lines
            .Where(predicate: x => x.TransactionType == TransactionType.Credit)
            .Sum(selector: x => x.Amount);

        return creditAmount == debitAmount;
    }

    public Result<JournalEntry> AddLine(Guid accountId, TransactionType type, decimal amount)
    {
        if (Status != JournalEntryStatus.Editable)
            return Result.Invalid(validationError: JournalEntryErrors.CannotEdit(status: Status));

        var line = JournalEntryLine.Create(
            type: type,
            amount: amount,
            accountId: accountId,
            entryId: Id);

        _lines.Add(item: line);
        return Result.Success(value: this);
    }

    /// <summary>
    ///     Makes the <see cref="JournalEntry" /> permanent in the system.
    /// </summary>
    /// <returns></returns>
    public Result Post()
    {
        if (Status == JournalEntryStatus.Posted)
            return Result.Invalid(validationError: JournalEntryErrors.AlreadyPosted());

        if (Status == JournalEntryStatus.Reversed)
            return Result.Invalid(validationError: JournalEntryErrors.CantPostBecauseIsReversed());

        var lineCount = _lines.Count;

        switch (lineCount)
        {
            case < 2:
                return Result.Invalid(validationError: JournalEntryErrors.LessThanTwoLines(lineCount: lineCount));

            case > 2 when Type != JournalEntryType.Compound:
                return Result.Invalid(
                    validationError: JournalEntryErrors.MoreThanTwoLinesWhenTypeIsNotCompound(lineCount: lineCount));

            case 2 when _lines[index: 0].TransactionType == _lines[index: 1].TransactionType:
                return Result.Invalid(
                    validationError: JournalEntryErrors.SameTransactionTypeOnBothLines(
                        type: _lines[index: 0].TransactionType));
        }

        if (!IsBalanced())
            return Result.Invalid(validationError: JournalEntryErrors.Imbalanced());

        Status = JournalEntryStatus.Posted;
        return Result.Success();
    }

    public Result Reverse()
    {
        Status = JournalEntryStatus.Reversed;
        return Result.Success();
    }
}