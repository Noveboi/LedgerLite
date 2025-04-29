using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Domain.JournalEntries;

public sealed class JournalEntryType(string name, int value) : SmartEnum<JournalEntryType>(name, value)
{
    /// <summary>
    /// A typical journal entry that records any kind of financial transaction.
    /// </summary>
    public static readonly JournalEntryType Standard = new(nameof(Standard), 0);
    
    /// <summary>
    /// A journal entry that is posted at regular time intervals.
    /// </summary>
    public static readonly JournalEntryType Recurring = new(nameof(Recurring), 1);
    
    /// <summary>
    /// Records end-of-period adjustments to comply with the accrual method of accounting.
    /// </summary>
    public static readonly JournalEntryType Adjusting = new(nameof(Adjusting), 2);
    
    /// <summary>
    /// "Reverses" adjusting journal entries to avoid double-counting.
    /// </summary>
    public static readonly JournalEntryType Reversing = new(nameof(Reversing), 3);
    
    /// <summary>
    /// Contains more than one credit or debit lines.
    /// </summary>
    public static readonly JournalEntryType Compound = new(nameof(Compound), 4);
    
    /// <summary>
    /// Used when opening and setting up new books.
    /// </summary>
    public static readonly JournalEntryType Opening = new(nameof(Opening), 5);
    
    /// <summary>
    /// Transfers temporary account balances (e.g: income, expenses) to a permanent
    /// equity account.
    /// </summary>
    public static readonly JournalEntryType Closing = new(nameof(Closing), 6);
}