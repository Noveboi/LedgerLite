using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class AccountNodeCreationTests
{
    private static readonly Guid ChartId = Guid.NewGuid();

    [Fact]
    public void Create_InitializesEmptyNode_WithNoChildrenOrParent()
    {
        var account = FakeAccounts.NewAccount();

        var node = AccountNode.Create(chartId: ChartId, account: account);

        node.ChartId.ShouldBe(expected: ChartId);
        node.Account.ShouldBe(expected: account);
        node.Parent.ShouldBeNull();
        node.ParentId.ShouldBeNull();
        node.Children.ShouldBeEmpty();
    }
}