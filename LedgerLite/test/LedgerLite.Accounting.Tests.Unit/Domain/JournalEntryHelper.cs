using Ardalis.Result;
using LedgerLite.Accounting.Domain.JournalEntries;

namespace LedgerLite.Accounting.Tests.Unit.Domain;

/// <summary>
/// Utility methods for testing journal entries.
/// </summary>
internal static class JournalEntryHelper
{
    public static Result<JournalEntry> Create(
        JournalEntryType type,
        IEnumerable<JournalEntryLine> lines)
    {
        return JournalEntry.Record(
            accountId: Guid.NewGuid(),
            type: type,
            referenceNumber: "",
            description: "",
            occuredAtUtc: DateTime.Today,
            lines: lines);
    }  
}