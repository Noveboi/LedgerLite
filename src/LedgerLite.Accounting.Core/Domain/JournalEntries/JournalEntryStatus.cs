using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

public sealed class JournalEntryStatus : SmartEnum<JournalEntryStatus>
{
    public static readonly JournalEntryStatus Editable = new(nameof(Editable), value: 1);
    public static readonly JournalEntryStatus Posted = new(nameof(Posted), value: 2);
    public static readonly JournalEntryStatus Reversed = new(nameof(Reversed), value: 3);

    private JournalEntryStatus() : this(name: "", value: 0)
    {
    }

    private JournalEntryStatus(string name, int value) : base(name: name, value: value)
    {
    }
}