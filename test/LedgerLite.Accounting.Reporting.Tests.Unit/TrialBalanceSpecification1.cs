using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Reporting.Trial;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Reporting.Tests.Unit;

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

    [Fact]
    public void ShouldBeBalanced()
    {
       var entries = new[] { _entry1, _entry2, _entry3 };
       var period = FakeFiscalPeriods.Get(x => x.WithId(_entry1.FiscalPeriodId));

       var result = TrialBalance.Prepare(period, entries);
       var trialBalance = result.Value;
       
       result.Status.ShouldBe(ResultStatus.Ok);
       trialBalance.GetTotalCredits().ShouldBe(330);
       trialBalance.GetTotalDebits().ShouldBe(330);
    }
}