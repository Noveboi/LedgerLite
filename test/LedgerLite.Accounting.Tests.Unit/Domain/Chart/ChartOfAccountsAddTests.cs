using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class ChartOfAccountsAddTests
{
    private readonly ChartOfAccounts _chart = FakeChartOfAccounts.Empty;
    
    [Fact]
    public void AddsToList()
    {
        var expected = FakeAccounts.NewAccount();

        var result = _chart.Add(expected);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        _chart.Accounts
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected);
    }

    [Fact]
    public void Invalid_WhenAccountAlreadyInChart()
    {
        var account = FakeAccounts.NewAccount();
        var chart = FakeChartOfAccounts.With(account);

        var result = chart.Add(account);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(ChartOfAccountsErrors.AccountAlreadyExists(account));
    }
}