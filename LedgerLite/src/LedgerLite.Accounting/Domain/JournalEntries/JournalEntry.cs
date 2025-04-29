using Ardalis.Result;
using LedgerLite.SharedKernel;

namespace LedgerLite.Accounting.Domain.JournalEntries;

/// <summary>
/// Records one financial transaction.
/// </summary>
public sealed class JournalEntry : AuditableEntity
{
    private JournalEntry() { }
    
    public string ReferenceNumber { get; private init; } = null!;
    public DateTime OccuredAtUtc { get; private init; }
    public string Description { get; private init; } = null!;
    public JournalEntryType Type { get; private init; } = null!;
    public JournalEntryStatus Status { get; private set; } = null!;
    
    private readonly List<JournalEntryLine> _lines = [];
    public IReadOnlyCollection<JournalEntryLine> Lines => _lines;

    /// <summary>
    /// Start recording a new journal entry. 
    /// </summary>
    public static Result<JournalEntry> Record(
        JournalEntryType type,
        string referenceNumber,
        string description,
        DateTime occuredAtUtc)
    {
        if (string.IsNullOrWhiteSpace(referenceNumber))
            return Result.Invalid(JournalEntryErrors.EmptyReferenceNumber());
        
        return Result.Success(new JournalEntry
        {
            Type = type,
            ReferenceNumber = referenceNumber,
            Description = description,
            OccuredAtUtc = occuredAtUtc,
            Status = JournalEntryStatus.Editable
        });
    }

    public Result AddLine(Guid accountId, TransactionType type, decimal amount)
    {
        if (amount <= 0)
            return Result.Invalid(JournalEntryErrors.NonPositiveAmount(amount));
        
        var line = JournalEntryLine.Create(
            type: type,
            amount: amount,
            accountId: accountId,
            entryId: Id);
        
        _lines.Add(line);
        return Result.Success();
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
        
        Status = JournalEntryStatus.Posted;
        return Result.Success();
    }

    public Result Reverse()
    {
        Status = JournalEntryStatus.Reversed;
        return Result.Success();
    }
}