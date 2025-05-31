using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

public sealed class JournalEntryType : SmartEnum<JournalEntryType>
{
    /// <summary>
    ///     A typical journal entry that records any kind of financial transaction.
    /// </summary>
    public static readonly JournalEntryType Standard = new(nameof(Standard), 1);

    /// <summary>
    ///     A journal entry that is posted at regular time intervals.
    /// </summary>
    public static readonly JournalEntryType Recurring = new(nameof(Recurring), 2);

    /// <summary>
    ///     Records end-of-period adjustments to comply with the accrual method of accounting.
    /// </summary>
    public static readonly JournalEntryType Adjusting = new(nameof(Adjusting), 3);

    /// <summary>
    ///     "Reverses" adjusting journal entries to avoid double-counting.
    /// </summary>
    public static readonly JournalEntryType Reversing = new(nameof(Reversing), 4);

    /// <summary>
    ///     Contains more than one credit or debit lines.
    /// </summary>
    public static readonly JournalEntryType Compound = new(nameof(Compound), 5);

    /// <summary>
    ///     Used when opening and setting up new books.
    /// </summary>
    public static readonly JournalEntryType Opening = new(nameof(Opening), 6);

    /// <summary>
    ///     Transfers temporary account balances (e.g: income, expenses) to a permanent
    ///     equity account.
    /// </summary>
    public static readonly JournalEntryType Closing = new(nameof(Closing), 7);

    private JournalEntryType() : this("", 0)
    {
    }

    private JournalEntryType(string name, int value) : base(name, value)
    {
    }
}