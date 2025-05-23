using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Reporting.TrialBalance;

internal sealed class TrialBalance
{
    public FiscalPeriod Period { get; }

    private TrialBalance(FiscalPeriod period)
    {
        Period = period;
    }

    public static Result<TrialBalance> Prepare(FiscalPeriod period, IReadOnlyList<JournalEntry> journalEntries)
    {
        if (journalEntries.Any(e => e.FiscalPeriodId != period.Id))
        {
            throw new InvalidOperationException($"Expected all entry periods to be {period.Id}.");
        }
        
        return new TrialBalance(period);
    }
}