using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

/// <summary>
/// Utility methods for testing journal entries.
/// </summary>
internal static class JournalEntryHelper
{
    private static readonly FiscalPeriod Period = FakeFiscalPeriods.Get();
    
    public static JournalEntry CreateWithLines(
        JournalEntryType type,
        IEnumerable<JournalEntryLine> lines)
    {
        var entry = JournalEntry.Create(
            type: type,
            referenceNumber: "abc123",
            description: "Testing!",
            occursAt: DateOnly.FromDateTime(DateTime.Today),
            createdByUserId: Guid.NewGuid(),
            fiscalPeriod: Period).Value;

        foreach (var line in lines)
        {
            entry.AddLine(line.AccountId, line.TransactionType, line.Amount);
        }

        return entry;
    }

    public static JournalEntry Create(JournalEntryType type) =>
        JournalEntry.Create(
            type: type,
            referenceNumber: "abc123",
            description: "Testing!",
            occursAt: DateOnly.FromDateTime(DateTime.Today),
            createdByUserId: Guid.NewGuid(),
            fiscalPeriod: Period).Value;
}