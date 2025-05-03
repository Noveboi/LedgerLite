using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

/// <summary>
/// Records one financial transaction.
/// </summary>
public sealed class JournalEntry : AuditableEntity
{
    private JournalEntry() { }
    
    public string ReferenceNumber { get; private init; } = null!;
    public DateTime OccursAtUtc { get; private init; }
    public string Description { get; private init; } = null!;
    public JournalEntryType Type { get; private init; } = null!;
    public JournalEntryStatus Status { get; private set; } = null!;
    
    private readonly List<JournalEntryLine> _lines = [];
    public IReadOnlyCollection<JournalEntryLine> Lines => _lines;

    /// <summary>
    /// Start recording a new journal entry. 
    /// </summary>
    public static Result<JournalEntry> Create(
        JournalEntryType type,
        string referenceNumber,
        string description,
        DateTime occursAtUtc)
    {
        if (string.IsNullOrWhiteSpace(referenceNumber))
            return Result.Invalid(JournalEntryErrors.EmptyReferenceNumber());
        
        return Result.Success(new JournalEntry
        {
            Type = type,
            ReferenceNumber = referenceNumber,
            Description = description,
            OccursAtUtc = occursAtUtc,
            Status = JournalEntryStatus.Editable
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