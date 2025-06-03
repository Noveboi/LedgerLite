using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Reporting.Trial;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Reporting.Tests.Unit;

/// <summary>
///     The journal entries are based on this video: <see href="https://www.youtube.com/watch?v=3_PfoTzSCQE" />
/// </summary>
public class TrialBalanceSpecification1
{
    private readonly JournalEntry _entry1 = FakeJournalEntries.Get(x => x
        .AddLine(o => o.Debit(account: CommonAccounts.Cash, amount: 100))
        .AddLine(o => o.Credit(account: CommonAccounts.OwnerEquity, amount: 100)));

    private readonly JournalEntry _entry2 = FakeJournalEntries.Get(x => x
        .AddLine(o => o.Debit(account: CommonAccounts.Cash, amount: 200))
        .AddLine(o => o.Credit(account: CommonAccounts.LoansPayable, amount: 200)));

    private readonly JournalEntry _entry3 = FakeJournalEntries.Get(x => x
        .AddLine(o => o.Credit(account: CommonAccounts.Cash, amount: 30))
        .AddLine(o => o.Debit(account: CommonAccounts.Equipment, amount: 30)));

    private readonly JournalEntry _entry4 = FakeJournalEntries.Get(x => x
        .AddLine(o => o.Debit(account: CommonAccounts.Supplies, amount: 50))
        .AddLine(o => o.Credit(account: CommonAccounts.AccountsPayable, amount: 50)));

    private readonly JournalEntry _entry5 = FakeJournalEntries.Get(x => x
        .WithType(journalEntryType: JournalEntryType.Compound)
        .AddLine(o => o.Debit(account: CommonAccounts.Cash, amount: 150))
        .AddLine(o => o.Credit(account: CommonAccounts.Revenue, amount: 150))
        .AddLine(o => o.Credit(account: CommonAccounts.Supplies, amount: 25))
        .AddLine(o => o.Debit(account: CommonAccounts.CostOfSales, amount: 25)));

    private readonly JournalEntry _entry6 = FakeJournalEntries.Get(x => x
        .AddLine(o => o.Credit(account: CommonAccounts.Cash, amount: 20))
        .AddLine(o => o.Debit(account: CommonAccounts.LaundryCosts, amount: 20)));

    [Fact]
    public void AccurateTotals_PerAccount()
    {
        var (period, entries) = Setup();

        var result = TrialBalance.Prepare(period: period, journalEntries: entries);
        var trialBalance = result.Value;

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        trialBalance.GetTotals(account: CommonAccounts.Cash).ShouldBe(expected: 400);
        trialBalance.GetTotals(account: CommonAccounts.OwnerEquity).ShouldBe(expected: 100);
        trialBalance.GetTotals(account: CommonAccounts.LoansPayable).ShouldBe(expected: 200);
        trialBalance.GetTotals(account: CommonAccounts.Revenue).ShouldBe(expected: 150);
        trialBalance.GetTotals(account: CommonAccounts.Supplies).ShouldBe(expected: 25);
        trialBalance.GetTotals(account: CommonAccounts.CostOfSales).ShouldBe(expected: 25);
        trialBalance.GetTotals(account: CommonAccounts.LaundryCosts).ShouldBe(expected: 20);
        trialBalance.GetTotals(account: CommonAccounts.Equipment).ShouldBe(expected: 30);
        trialBalance.GetTotals(account: CommonAccounts.AccountsPayable).ShouldBe(expected: 50);
    }

    [Fact]
    public void ShouldBeBalanced()
    {
        var (period, entries) = Setup();

        var result = TrialBalance.Prepare(period: period, journalEntries: entries);
        var trialBalance = result.Value;

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        trialBalance.GetTotalCredits().ShouldBe(expected: 500);
        trialBalance.GetTotalDebits().ShouldBe(expected: 500);
    }

    private (FiscalPeriod, IReadOnlyList<JournalEntry>) Setup()
    {
        var entries = new[] { _entry1, _entry2, _entry3, _entry4, _entry5, _entry6 };
        var period = FakeFiscalPeriods.Get(x => x.WithId(id: _entry1.FiscalPeriodId));

        return (period, entries);
    }
}