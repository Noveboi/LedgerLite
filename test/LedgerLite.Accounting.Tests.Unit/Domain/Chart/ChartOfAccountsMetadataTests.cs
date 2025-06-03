using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Accounts.Metadata;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class ChartOfAccountsMetadataTests
{
    private Account _parent = FakeAccounts.GetPlaceholder(x => x
        .WithType(AccountType.Expense)
        .WithExpenseType(ExpenseType.Indirect));
    
    [Fact]
    public void Move_InvalidWhenExpenseTypes_DoNotMatch()
    {
        var (account, chart) = GetAccountAndChart(x => x
            .WithType(AccountType.Expense)
            .WithExpenseType(ExpenseType.Direct));
        
        var result = chart.Move(accountId: account.Id, parentId: _parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(ChartOfAccountsErrors.AccountInvalidExpenseType(_parent, account));
    }

    [Fact]
    public void Move_ValidWhenExpenseTypes_Match()
    {
        var (account, chart) = GetAccountAndChart(x => x
            .WithType(AccountType.Expense)
            .WithExpenseType(ExpenseType.Indirect));

        var result = chart.Move(accountId: account.Id, parentId: _parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Move_ShouldSetExpenseTypeOfAccount_WhenItIsUndefined()
    {
        var (account, chart) = GetAccountAndChart(x => x
            .WithType(AccountType.Expense)
            .WithExpenseType(ExpenseType.Undefined));

        var result = chart.Move(accountId: account.Id, parentId: _parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        account.Metadata.ExpenseType.ShouldBe(_parent.Metadata.ExpenseType);
    }

    [Theory]
    [InlineData(ExpenseType.Indirect)]
    [InlineData(ExpenseType.Direct)]
    [InlineData(ExpenseType.Undefined)]
    public void Move_UndefinedExpenseTypeParent_ShouldNotAffectType_OfTarget(ExpenseType expenseType)
    {
        var (account, chart) = GetAccountAndChart(
            options: x => x.WithType(AccountType.Expense).WithExpenseType(expenseType),
            parentOptions: x => x.WithType(AccountType.Expense).WithExpenseType(ExpenseType.Undefined));
        
        var result = chart.Move(accountId: account.Id, parentId: _parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        account.Metadata.ExpenseType.ShouldBe(expenseType);
    }

    private (Account, ChartOfAccounts) GetAccountAndChart(
        Action<FakeAccountOptions> options,
        Action<FakeAccountOptions>? parentOptions = null)
    {
        var account = FakeAccounts.Get(options);
        _parent = parentOptions != null ? FakeAccounts.GetPlaceholder(parentOptions) : _parent;

        return (account, FakeChartOfAccounts.With(_parent, account));
    }
}