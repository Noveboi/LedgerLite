using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Accounts.Metadata;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Accounts;

public class AccountMetadataTests
{
    [Fact]
    public void ExpenseType_CanBeIndirect_WhenAccountTypeIsExpense()
    {
        var metadata = new AccountMetadata(ExpenseType: ExpenseType.Indirect);
        var result = metadata.Verify(accountType: AccountType.Expense);
        result.Status.ShouldBe(expected: ResultStatus.Ok);
    }

    [Fact]
    public void ExpenseType_CannotBeInDirect_WhenAccountTypeIsNotExpense()
    {
        var metadata = new AccountMetadata(ExpenseType: ExpenseType.Indirect);
        
        var result = metadata.Verify(accountType: AccountType.Asset);
        
        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ShouldHaveError(AccountMetadataErrors.OnlyExpensesCanBeIndirect(accountType: AccountType.Asset));
    }
}