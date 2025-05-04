using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class ChartOfAccountTests
{
    private readonly ChartOfAccounts _chart = FakeChartOfAccounts.Empty;
    
    [Fact]
    public void Add_AddsToList()
    {
        var expected = FakeAccounts.Get(o => o.IsPlaceholder = true);

        var result = _chart.Add(expected);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        _chart.Accounts
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected);
    }
    
    [Fact]
    public void Attach_Invalid_WhenAccountIsNotPlaceholder()
    {
        var parent = FakeAccounts.Get(o => o.IsPlaceholder = false);
        var account = FakeAccounts.NewAccount();
        var chart = FakeChartOfAccounts.With(parent);

        var result = chart.Attach(account: account, parentId: parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.NoChildrenWhenNotPlaceholder());
    }

    [Fact]
    public void Attach_Invalid_WhenChildAccount_IsNotOfTheSameType()
    {
        var parent = FakeAccounts.Get(o =>
        {
            o.IsPlaceholder = true;
            o.Type = AccountType.Asset;
        });
        var child = FakeAccounts.Get(o => o.Type = AccountType.Expense);
        var chart = FakeChartOfAccounts.With(parent);

        var result = chart.Attach(account: child, parentId: parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.ChildHasDifferentType(parent.Type, child.Type));
    }

    [Fact]
    public void Attach_Invalid_WhenAddingAccountToItself()
    {
        var account = FakeAccounts.Get(o => o.IsPlaceholder = true);
        var chart = FakeChartOfAccounts.With(account);

        var result = chart.Attach(account, account.Id);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.AddAccountToItself());
    }
    
    [Fact]
    public void Attach_AppendsNewAccount_ToChildren_WhenValid()
    {
        var parent = FakeAccounts.Get(o => o.IsPlaceholder = true);
        var child = FakeAccounts.Get(o => o.Type = parent.Type);
        var chart = FakeChartOfAccounts.With(parent);

        var result = chart.Attach(account: child, parentId: parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        chart.Accounts.Count().ShouldBe(2);
        chart.Accounts.ShouldContain(parent);
        chart.Accounts.ShouldContain(child);
    }

    [Fact]
    public void Attach_ReturnsNotFound_WhenParentDoesNotExist()
    {
        var child = FakeAccounts.NewAccount();

        var result = _chart.Attach(account: child, parentId: Guid.CreateVersion7());
        
        result.Status.ShouldBe(ResultStatus.NotFound);
        _chart.Accounts.ShouldBeEmpty();
    }
}