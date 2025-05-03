using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class ChartOfAccountTests
{
    private static readonly Guid UserId = Guid.NewGuid();
    
    [Fact]
    public void Add_AddsToList()
    {
        var chart = ChartOfAccounts.Create(UserId);
        var account = FakeAccounts.NewAccount();

        var result = chart.AddAccount(account);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        chart.Accounts
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(account);
    }
    
    [Fact]
    public void Add_AllowOnlyRootLevelAccounts()
    {
        var chart = ChartOfAccounts.Create(UserId);
        var account = FakeAccounts.Get(o => o.HierarchyLevel = 1);

        var result = chart.AddAccount(account);
        
        chart.Accounts.ShouldBeEmpty();
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(ChartOfAccountsErrors.AccountNotRootLevel());
    }

    [Fact]
    public void Remove_NotFound_WhenAccountDoesNotExist()
    {
        var chart = ChartOfAccounts.Create(UserId);
        var account = FakeAccounts.NewAccount();

        var result = chart.RemoveAccount(account);
        
        result.Status.ShouldBe(ResultStatus.NotFound);
    }

    [Fact]
    public void Remove_RemovesFromList()
    {
        var chart = ChartOfAccounts.Create(UserId);
        var account = FakeAccounts.NewAccount();
        chart.AddAccount(account);

        var result = chart.RemoveAccount(account);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        chart.Accounts.ShouldBeEmpty();
    }
}