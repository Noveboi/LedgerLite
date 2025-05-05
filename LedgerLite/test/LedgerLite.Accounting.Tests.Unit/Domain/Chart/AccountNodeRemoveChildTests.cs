using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class AccountNodeRemoveChildTests
{
    [Fact]
    public void Invalid_WhenChildDoesNotExist_InChildren()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;
        node.AddChild(FakeAccountNodes.Get());

        var result = node.RemoveChild(child);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(ChartOfAccountsErrors.AccountNotChild(node.Account, child.Account));
    }

    [Fact]
    public void Invalid_WhenNoChildren()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;

        var result = node.RemoveChild(child);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(ChartOfAccountsErrors.AccountHasNoChildrenToRemove(node.Account));
    }

    [Fact]
    public void RemovesNodeFromChildrenList()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;
        node.AddChild(child);
        var childrenBefore = node.Children.ToList();

        var result = node.RemoveChild(child);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        childrenBefore.ShouldHaveSingleItem();
        node.Children.ShouldBeEmpty();
    }

    [Fact]
    public void RemovesParentReference_FromChildNode()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;
        node.AddChild(child);

        var result = node.RemoveChild(child);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        node.Parent.ShouldBeNull();
    }
}