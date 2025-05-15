using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class AccountNodeCreationTests
{
    private static readonly Guid ChartId = Guid.NewGuid();
    
    [Fact]
    public void Create_InitializesEmptyNode_WithNoChildrenOrParent()
    {
        var account = FakeAccounts.NewAccount();

        var node = AccountNode.Create(ChartId, account);
        
        node.ChartId.ShouldBe(ChartId);
        node.Account.ShouldBe(account);
        node.Parent.ShouldBeNull();
        node.ParentId.ShouldBeNull();
        node.Children.ShouldBeEmpty();
    }
}