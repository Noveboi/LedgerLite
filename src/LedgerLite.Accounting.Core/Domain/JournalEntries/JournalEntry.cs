using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

/// <summary>
/// Records one financial transaction.
/// </summary>
public sealed class JournalEntry : AuditableEntity
{
    private JournalEntry() { }
    
    public Guid FiscalPeriodId { get; private init; }
    public Guid CreatedByUserId { get; private init;}
    public Guid? LastModifiedByUserId { get; private init; }
    
    public string ReferenceNumber { get; private init; } = null!;
    public DateOnly OccursAt { get; private init; }
    public string Description { get; private init; } = null!;
    public JournalEntryType Type { get; private init; } = null!;
    public JournalEntryStatus Status { get; private set; } = null!;
    
    private readonly List<JournalEntryLine> _lines = [];
    public IReadOnlyCollection<JournalEntryLine> Lines => _lines;

    /// <summary>
    /// Start recording a new journal entry. 
    /// </summary>
    public static Result<JournalEntry> Create(
        FiscalPeriod fiscalPeriod,
        JournalEntryType type,
        string referenceNumber,
        string description,
        DateOnly occursAt,
        Guid createdByUserId)
    {
        if (string.IsNullOrWhiteSpace(referenceNumber))
            return Result.Invalid(JournalEntryErrors.EmptyReferenceNumber());

        if (fiscalPeriod.IsClosed)
            return Result.Invalid(JournalEntryErrors.CannotEditBecausePeriodIsClosed(fiscalPeriod));
        
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
            return Result.Invalid(JournalEntryErrors.CannotEdit(Status));
        
        var line = JournalEntryLine.Create(
            type: type,
            amount: amount,
            accountId: accountId,
            entryId: Id);
        
        _lines.Add(line);
        return Result.Success(this);
    }

    /// <summary>
    /// Makes the <see cref="JournalEntry"/> permanent in the system.
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
                return Result.Invalid(JournalEntryErrors.LessThanTwoLines(lineCount));
            
            case > 2 when Type != JournalEntryType.Compound:
                return Result.Invalid(JournalEntryErrors.MoreThanTwoLinesWhenTypeIsNotCompound(lineCount));
            
            case 2 when _lines[0].TransactionType == _lines[1].TransactionType:
                return Result.Invalid(JournalEntryErrors.SameTransactionTypeOnBothLines(_lines[0].TransactionType));
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