using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Reporting.TrialBalance;

internal sealed record TrialBalance()
{
    public static TrialBalance Prepare(IEnumerable<JournalEntry> journalEntries)
    {
        return new TrialBalance();
    }
}