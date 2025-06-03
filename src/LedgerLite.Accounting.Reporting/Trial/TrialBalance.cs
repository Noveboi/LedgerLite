using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;

namespace LedgerLite.Accounting.Reporting.Trial;

internal sealed class TrialBalance
{
    private TrialBalance(FiscalPeriod period, IReadOnlyCollection<AccountBalance> workingBalance)
    {
        Period = period;
        WorkingBalance = workingBalance;
    }

    public FiscalPeriod Period { get; }
    public IReadOnlyCollection<AccountBalance> WorkingBalance { get; }

    public decimal GetTotals(Account account)
    {
        return WorkingBalance.FirstOrDefault(x => x.Account == account)?.Amount ?? 0;
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

    public static Result<TrialBalance> Prepare(FiscalPeriod period, IReadOnlyList<JournalEntry> journalEntries)
    {
        if (journalEntries.Any(e => e.FiscalPeriodId != period.Id))
            throw new InvalidOperationException($"Expected all entry periods to be {period.Id}.");

        var workingBalance = journalEntries
            .SelectMany(entry => entry.Lines)
            .GroupBy(line => line.Account)
            .Select(AccountBalance.FromGroup);

        return new TrialBalance(
            period: period, 
            workingBalance.ToList());
    }
}