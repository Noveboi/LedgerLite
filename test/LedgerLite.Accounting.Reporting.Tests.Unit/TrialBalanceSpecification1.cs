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
        .AddLine(o => o.Debit(CommonAccounts.Cash, 100))
        .AddLine(o => o.Credit(CommonAccounts.OwnerEquity, 100)));

    private readonly JournalEntry _entry2 = FakeJournalEntries.Get(x => x
        .AddLine(o => o.Debit(CommonAccounts.Cash, 200))
        .AddLine(o => o.Credit(CommonAccounts.LoansPayable, 200)));

    private readonly JournalEntry _entry3 = FakeJournalEntries.Get(x => x
        .AddLine(o => o.Credit(CommonAccounts.Cash, 30))
        .AddLine(o => o.Debit(CommonAccounts.Equipment, 30)));

    private readonly JournalEntry _entry4 = FakeJournalEntries.Get(x => x
        .AddLine(o => o.Debit(CommonAccounts.Supplies, 50))
        .AddLine(o => o.Credit(CommonAccounts.AccountsPayable, 50)));

    private readonly JournalEntry _entry5 = FakeJournalEntries.Get(x => x
        .WithType(JournalEntryType.Compound)
        .AddLine(o => o.Debit(CommonAccounts.Cash, 150))
        .AddLine(o => o.Credit(CommonAccounts.Revenue, 150))
        .AddLine(o => o.Credit(CommonAccounts.Supplies, 25))
        .AddLine(o => o.Debit(CommonAccounts.CostOfSales, 25)));

    private readonly JournalEntry _entry6 = FakeJournalEntries.Get(x => x
        .AddLine(o => o.Credit(CommonAccounts.Cash, 20))
        .AddLine(o => o.Debit(CommonAccounts.LaundryCosts, 20)));

    [Fact]
    public void AccurateTotals_PerAccount()
    {
        var (period, entries) = Setup();

        var result = TrialBalance.Prepare(period, entries);
        var trialBalance = result.Value;

        result.Status.ShouldBe(ResultStatus.Ok);
        trialBalance.GetTotals(CommonAccounts.Cash).ShouldBe(400);
        trialBalance.GetTotals(CommonAccounts.OwnerEquity).ShouldBe(100);
        trialBalance.GetTotals(CommonAccounts.LoansPayable).ShouldBe(200);
        trialBalance.GetTotals(CommonAccounts.Revenue).ShouldBe(150);
        trialBalance.GetTotals(CommonAccounts.Supplies).ShouldBe(25);
        trialBalance.GetTotals(CommonAccounts.CostOfSales).ShouldBe(25);
        trialBalance.GetTotals(CommonAccounts.LaundryCosts).ShouldBe(20);
        trialBalance.GetTotals(CommonAccounts.Equipment).ShouldBe(30);
        trialBalance.GetTotals(CommonAccounts.AccountsPayable).ShouldBe(50);
    }

    [Fact]
    public void ShouldBeBalanced()
    {
        var (period, entries) = Setup();

        var result = TrialBalance.Prepare(period, entries);
        var trialBalance = result.Value;

        result.Status.ShouldBe(ResultStatus.Ok);
        trialBalance.GetTotalCredits().ShouldBe(500);
        trialBalance.GetTotalDebits().ShouldBe(500);
    }

    private (FiscalPeriod, IReadOnlyList<JournalEntry>) Setup()
    {
        var entries = new[] { _entry1, _entry2, _entry3, _entry4, _entry5, _entry6 };
        var period = FakeFiscalPeriods.Get(x => x.WithId(_entry1.FiscalPeriodId));

        return (period, entries);
    }
}