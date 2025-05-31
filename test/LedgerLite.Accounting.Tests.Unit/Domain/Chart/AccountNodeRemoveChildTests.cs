using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class AccountNodeRemoveChildTests
{
    [Fact]
    public void Invalid_WhenChildDoesNotExist_InChildren()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;
        node.AddChild(child: FakeAccountNodes.Get());

        var result = node.RemoveChild(child: child);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(
                expected: ChartOfAccountsErrors.AccountNotChild(parent: node.Account, child: child.Account));
    }

    [Fact]
    public void Invalid_WhenNoChildren()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;

        var result = node.RemoveChild(child: child);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: ChartOfAccountsErrors.AccountHasNoChildrenToRemove(account: node.Account));
    }

    [Fact]
    public void RemovesNodeFromChildrenList()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;
        node.AddChild(child: child);
        var childrenBefore = node.Children.ToList();

        var result = node.RemoveChild(child: child);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        childrenBefore.ShouldHaveSingleItem();
        node.Children.ShouldBeEmpty();
    }

    [Fact]
    public void RemovesParentReference_FromChildNode()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;
        node.AddChild(child: child);

        var result = node.RemoveChild(child: child);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        node.Parent.ShouldBeNull();
    }
}