using LedgerLite.Accounting.Domain.JournalEntries;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

/// <summary>
/// Utility methods for testing journal entries.
/// </summary>
internal static class JournalEntryHelper
{
    public static JournalEntry CreateWithLines(
        JournalEntryType type,
        IEnumerable<JournalEntryLine> lines)
    {
        var entry = JournalEntry.Record(
            type: type,
            referenceNumber: "abc123",
            description: "Testing!",
            occuredAtUtc: DateTime.Today).Value;

        foreach (var line in lines)
        {
            entry.AddLine(line.AccountId, line.TransactionType, line.Amount);
        }

        return entry;
    }

    public static JournalEntry Create(JournalEntryType type) =>
        JournalEntry.Record(
            type: type,
            referenceNumber: "abc123",
            description: "Testing!",
            occuredAtUtc: DateTime.Today).Value;
}