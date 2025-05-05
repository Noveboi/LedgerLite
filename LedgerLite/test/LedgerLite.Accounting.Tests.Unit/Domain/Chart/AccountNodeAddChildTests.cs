using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class AccountNodeAddChildTests
{
    
    [Fact]
    public void Invalid_WhenAddingAccountAsItsOwnChild()
    {
        var node = FakeAccountNodes.Get();
        
        var result = node.AddChild(node);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.AddAccountToItself());
    }

    [Fact]
    public void Invalid_WhenParentIsNotAPlaceholderAccount()
    {
        var node = FakeAccountNodes.Get(o => o.IsPlaceholder = false);

        var result = node.AddChild(FakeAccountNodes.SampleChild);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.NoChildrenWhenNotPlaceholder());
    }

    [Fact]
    public void Invalid_WhenParentAndChildAccountTypes_AreNotTheSame()
    {
        var node = FakeAccountNodes.Get(o =>
        {
            o.Type = AccountType.Asset;
            o.IsPlaceholder = true;
        });
        var child = FakeAccountNodes.Get(o => o.Type = AccountType.Equity);

        var result = node.AddChild(child);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.ChildHasDifferentType(
                expected: node.Account.Type,
                actual: child.Account.Type));
    }

    [Fact]
    public void Invalid_WhenChildAlreadyExists()
    {
        var node = FakeAccountNodes.Get();
        node.AddChild(FakeAccountNodes.SampleChild);

        var result = node.AddChild(FakeAccountNodes.SampleChild);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(ChartOfAccountsErrors.AccountAlreadyExists(FakeAccountNodes.SampleChild.Account));
    }

    [Fact]
    public void Parent_ShouldHaveNewChild_InChildren()
    {
        var node = FakeAccountNodes.Get();

        var result = node.AddChild(FakeAccountNodes.SampleChild);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        node.Children
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(FakeAccountNodes.SampleChild);
    }

    [Fact]
    public void Child_ShouldHaveParent()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;

        var result = node.AddChild(child);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        child.Parent.ShouldBeEquivalentTo(node);
        child.ParentId.ShouldBeEquivalentTo(node.Id);
    }
}