using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.SharedKernel.Domain;
using LedgerLite.SharedKernel.Domain.Errors;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
    public Guid? LastModifiedByUserId { get; private set; }

    public string ReferenceNumber { get; private init; } = null!;
    public DateOnly OccursAt { get; private set; }
    public string Description { get; private set; } = null!;
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
            return Result.Invalid(JournalEntryErrors.EmptyReferenceNumber());

        if (fiscalPeriod.IsClosed)
            return Result.Invalid(
                JournalEntryErrors.CannotEditBecausePeriodIsClosed(period: fiscalPeriod));

        return Result.Success(new JournalEntry
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

    public Result Update(
        Guid userId,
        string? description,
        DateOnly? occursAt,
        UpdateLineRequest? lineRequest)
    {
        if (_lines.Count > 2)
            throw new NotSupportedException("Updating compound entries is not supported.");
        
        if (lineRequest != null)
        {
            if (_lines.FirstOrDefault(x => x.Id == lineRequest.LineId) is not { } line)
            {
                return Result.NotFound(CommonErrors.NotFound<JournalEntryLine>(lineRequest.LineId));
            }

            line.Update(lineRequest);
        }
        
        if (description != null)
            Description = description;

        if (occursAt != null)
            OccursAt = occursAt.Value;
        
        LastModifiedByUserId = userId;

        return Result.Success();
    }

    public bool IsBalanced()
    {
        var debitAmount = _lines
            .Where(x => x.TransactionType == TransactionType.Debit)
            .Sum(x => x.Amount);

        var creditAmount = _lines
            .Where(x => x.TransactionType == TransactionType.Credit)
            .Sum(x => x.Amount);

        return creditAmount == debitAmount;
    }

    public Result<JournalEntry> AddLine(Guid accountId, TransactionType type, decimal amount)
    {
        if (Status != JournalEntryStatus.Editable)
            return Result.Invalid(JournalEntryErrors.CannotEdit(status: Status));

        var line = JournalEntryLine.Create(
            type: type,
            amount: amount,
            accountId: accountId,
            entryId: Id);

        _lines.Add(item: line);
        return Result.Success(this);
    }

    /// <summary>
    ///     Makes the <see cref="JournalEntry" /> permanent in the system.
    /// </summary>
    /// <returns></returns>
    public Result Post()
    {
        if (Status == JournalEntryStatus.Posted)
            return Result.Invalid(JournalEntryErrors.AlreadyPosted());

        if (Status == JournalEntryStatus.Reversed)
            return Result.Invalid(JournalEntryErrors.CantPostBecauseIsReversed());

        var lineCount = _lines.Count;

        switch (lineCount)
        {
            case < 2:
                return Result.Invalid(JournalEntryErrors.LessThanTwoLines(lineCount: lineCount));

            case > 2 when Type != JournalEntryType.Compound:
                return Result.Invalid(
                    JournalEntryErrors.MoreThanTwoLinesWhenTypeIsNotCompound(lineCount: lineCount));

            case 2 when _lines[index: 0].TransactionType == _lines[index: 1].TransactionType:
                return Result.Invalid(
                    JournalEntryErrors.SameTransactionTypeOnBothLines(
                        type: _lines[index: 0].TransactionType));
        }

        if (!IsBalanced())
            return Result.Invalid(JournalEntryErrors.Imbalanced());

        Status = JournalEntryStatus.Posted;
        return Result.Success();
    }

    public Result Reverse()
    {
        Status = JournalEntryStatus.Reversed;
        return Result.Success();
    }
}