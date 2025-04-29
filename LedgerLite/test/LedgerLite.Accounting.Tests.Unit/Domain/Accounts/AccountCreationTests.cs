using LedgerLite.Accounting.Domain;
using LedgerLite.Accounting.Domain.Accounts;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Accounts;

public class AccountCreationTests
{
    [Fact]
    public void Initialize_WithNullParentId_AndEmptyChildList()
    {
        var account = Account.Create("Test", "1", AccountType.Asset, Currency.Euro, false);
        
        account.ParentAccountId.ShouldBeNull();
        account.ChildAccounts.ShouldBeEmpty();
    }
}