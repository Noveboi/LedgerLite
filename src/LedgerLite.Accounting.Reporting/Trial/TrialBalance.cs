using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Reporting.Trial;

internal sealed class TrialBalance
{
    private TrialBalance(IReadOnlyCollection<AccountBalance> workingBalance)
    {
        WorkingBalance = workingBalance;
    }

    public IReadOnlyCollection<AccountBalance> WorkingBalance { get; }

    public static TrialBalance Prepare(IEnumerable<JournalEntryLine> lines)
    {
        lines = lines as IReadOnlyList<JournalEntryLine> ?? lines;
        
        var workingBalance = lines
            .GroupBy(line => line.Account)
            .Select(AccountBalance.FromGroup);
        
        return new TrialBalance(workingBalance: workingBalance.ToList());
    }
    
    public static Result<TrialBalance> Prepare(FiscalPeriod period, IReadOnlyList<JournalEntry> journalEntries)
    {
        if (journalEntries.Any(e => e.FiscalPeriodId != period.Id))
            throw new InvalidOperationException($"Expected all entry periods to be {period.Id}.");

        return Prepare(journalEntries.SelectMany(x => x.Lines));
    }
    
    public decimal GetTotals(Account account)
    {
        return WorkingBalance.FirstOrDefault(x => x.Account == account)?.Amount ?? 0;
    }

    public decimal GetTotals(Func<AccountBalance, bool> predicate)
    {
        return WorkingBalance.Where(predicate).Sum(x => x.Amount);
    }

    public decimal GetTotalDebits()
    {
        return WorkingBalance
            .Where(x => x.Type == TransactionType.Debit)
            .Sum(x => x.Amount);
    }

    public decimal GetTotalCredits()
    {
        return WorkingBalance
            .Where(x => x.Type == TransactionType.Credit)
            .Sum(x => x.Amount);
    }
}