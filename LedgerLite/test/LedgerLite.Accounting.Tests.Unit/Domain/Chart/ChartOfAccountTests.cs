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

        var result = _chart.AddRootAccount(expected);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        var node = _chart.Accounts.ShouldHaveSingleItem();
        node.Account.ShouldBeEquivalentTo(expected);
        node.Parent.ShouldBeNull();
    }
    
        [Fact]
    public void AddWithParent_Invalid_WhenAccountIsNotPlaceholder()
    {
        var parent = FakeAccounts.Get(o => o.IsPlaceholder = false);
        var account = FakeAccounts.NewAccount();

        var result = _chart.AddAccountWithParent(account: account, parent: parent);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.NoChildrenWhenNotPlaceholder());
    }

    [Fact]
    public void AddWithParent_Invalid_WhenChildAccount_IsNotOfTheSameType()
    {
        var parent = FakeAccounts.Get(o =>
        {
            o.IsPlaceholder = true;
            o.Type = AccountType.Asset;
        });
        var child = FakeAccounts.Get(o => o.Type = AccountType.Expense);

        var result = _chart.AddAccountWithParent(account: child, parent: parent);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.ChildHasDifferentType(parent.Type, child.Type));
    }

    [Fact]
    public void Add_Invalid_WhenAddingAccountToItself()
    {
        var account = FakeAccounts.NewAccount();

        var result = _chart.AddAccountWithParent(account, account);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.AddAccountToItself());
    }
    
    [Fact]
    public void AddWithParent_AppendsNewAccount_ToChildren_WhenValid()
    {
        var account = FakeAccounts.Get(o => o.IsPlaceholder = true);
        var child = FakeAccounts.Get(o => o.Type = account.Type);

        var result = _chart.AddAccountWithParent(account: child, parent: account);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        _chart.Accounts
            .ShouldHaveSingleItem()
            .Account
            .ShouldBeEquivalentTo(child);
    }

    [Fact]
    public void Add_AssignsParent_ToChildAccount()
    {
        var account = FakeAccounts.Get(o => o.IsPlaceholder = true);
        var child = FakeAccounts.Get(o => o.Type = account.Type);

        var result = _chart.AddAccountWithParent(account: child, parent: account);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        var node = _chart.Accounts.ShouldHaveSingleItem();
        node.Parent.ShouldBeEquivalentTo(account);
        node.ParentId.ShouldBeEquivalentTo(account.Id);
    }
}