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
        var metadata = new AccountMetadata(ExpenseType.Indirect);
        var result = metadata.Verify(AccountType.Expense);
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void ExpenseType_CannotBeInDirect_WhenAccountTypeIsNotExpense()
    {
        var metadata = new AccountMetadata(ExpenseType.Indirect);
        
        var result = metadata.Verify(AccountType.Asset);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(AccountMetadataErrors.OnlyExpensesCanBeIndirect(AccountType.Asset));
    }
}