using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Tests.Unit.Utilities;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Accounts;

public class AccountChildTests
{
    [Fact]
    public void Add_Invalid_WhenAccountIsNotPlaceholder()
    {
        var accounts = FakeAccounts.NewAccounts(2);
        var (account1, account2) = (accounts[0], accounts[1]);

        var result = account1.AddChildAccount(account2);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.NoChildrenWhenNotPlaceholder());
    }

    [Fact]
    public void Add_Invalid_WhenChildAccount_IsNotOfTheSameType()
    {
        var account = FakeAccounts.Get(o =>
        {
            o.Children = [];
            o.Type = AccountType.Asset;
        });
        var child = FakeAccounts.Get(o => o.Type = AccountType.Expense);

        var result = account.AddChildAccount(child);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.ChildHasDifferentType(account.Type, child.Type));
    }

    [Fact]
    public void Add_Invalid_WhenAddingAccountToItself()
    {
        var account = FakeAccounts.Get(o => o.Children = []);

        var result = account.AddChildAccount(account);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.AddAccountToItself());
    }
    
    [Fact]
    public void Add_AppendsNewAccount_ToChildren_WhenValid()
    {
        var account = FakeAccounts.Get(o => o.Children = []);
        var child = FakeAccounts.Get(o => o.Type = account.Type);

        var result = account.AddChildAccount(child);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        account.ChildAccounts
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(child);
    }

    [Fact]
    public void Add_AssignsParentId_ToChildAccount()
    {
        var account = FakeAccounts.Get(o => o.Children = []);
        var child = FakeAccounts.Get(o => o.Type = account.Type);

        var result = account.AddChildAccount(child);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        child.ParentAccountId.ShouldBe(account.Id);
    }

    [Fact]
    public void Remove_NotFound_WhenAccountNotInChildren()
    {
        var account = FakeAccounts.Get(o => o.Children = []);
        var child = FakeAccounts.Get(o => o.Type = account.Type);

        var result = account.RemoveChildAccount(child);
        
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Errors
            .ShouldHaveSingleItem()
            .ShouldContain(account.Name);
    }

    [Fact]
    public void Remove_RemovesChildFromList()
    {
        var child = FakeAccounts.Get(o => o.Type = AccountType.Asset);
        var account = FakeAccounts.Get(o =>
        {
            o.Children = [child];
            o.Type = AccountType.Asset;
        });

        var result = account.RemoveChildAccount(child);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        account.ChildAccounts.ShouldBeEmpty();
    }

    [Fact]
    public void Remove_ClearParentId_OfChild()
    {
        var child = FakeAccounts.Get(o => o.Type = AccountType.Equity);
        var account = FakeAccounts.Get(o =>
        {
            o.Children = [child];
            o.Type = AccountType.Equity;
        });

        var result = account.RemoveChildAccount(child);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        child.ParentAccountId.ShouldBeNull();
    }

    [Fact]
    public void Add_IncrementsHierarchyLevel_OfChildAccount()
    {
        var account = FakeAccounts.Get(o => o.Children = []);
        var child = FakeAccounts.Get(o => o.Type = account.Type);

        account.AddChildAccount(child);
        
        child.HierarchyLevel.ShouldBe(account.HierarchyLevel + 1);
    }

    [Fact]
    public void Remove_SetsHierarchyLevel_ToZero()
    {
        var child = FakeAccounts.Get(o => o.Type = AccountType.Equity);
        var account = FakeAccounts.Get(o =>
        {
            o.Children = [child];
            o.Type = AccountType.Equity;
        });

        account.RemoveChildAccount(child);
        
        child.HierarchyLevel.ShouldBe(0);
    }
}