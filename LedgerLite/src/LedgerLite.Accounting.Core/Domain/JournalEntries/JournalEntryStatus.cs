using Ardalis.SmartEnum;

namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

public sealed class JournalEntryStatus(string name, int value) : SmartEnum<JournalEntryStatus>(name, value)
{
    public static readonly JournalEntryStatus Editable = new(nameof(Editable), 1);
    public static readonly JournalEntryStatus Posted = new(nameof(Posted), 2);
    public static readonly JournalEntryStatus Reversed = new(nameof(Reversed), 3);
}