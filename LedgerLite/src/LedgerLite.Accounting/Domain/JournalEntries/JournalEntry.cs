using System.Data;
using Ardalis.Result;
using LedgerLite.SharedKernel;

namespace LedgerLite.Accounting.Domain.JournalEntries;

/// <summary>
/// Records one financial transaction.
/// </summary>
public sealed class JournalEntry : Entity
{
    private JournalEntry(List<JournalEntryLine> lines)
    {
        _lines = lines;
    }
    
    public Guid AccountId { get; private init; }
    public string ReferenceNumber { get; private init; } = null!;
    public DateTime OccuredAtUtc { get; private init; }
    public string Description { get; private init; } = null!;
    public JournalEntryType Type { get; private init; } = null!;
    
    private readonly List<JournalEntryLine> _lines;
    public IReadOnlyCollection<JournalEntryLine> Lines => _lines;

    public static Result<JournalEntry> Record(
        Guid accountId,
        JournalEntryType type,
        string referenceNumber,
        string description,
        DateTime occuredAtUtc,
        IEnumerable<JournalEntryLine> lines)
    {
        var lineList = lines.ToList();
        var lineCount = lineList.Count;

        if (lineCount < 2)
            return Result.Invalid(JournalEntryErrors.LessThanTwoLines(lineCount));

        if (lineCount > 2 && type != JournalEntryType.Compound)
            return Result.Invalid(JournalEntryErrors.MoreThanTwoLinesWhenTypeIsNotCompound(lineCount));

        return Result.Success(new JournalEntry(lineList)
        {
            AccountId = accountId,
            Type = type,
            ReferenceNumber = referenceNumber,
            Description = description,
            OccuredAtUtc = occuredAtUtc
        });
    }
}