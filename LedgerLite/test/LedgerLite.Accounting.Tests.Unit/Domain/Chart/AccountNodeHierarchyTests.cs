using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class AccountNodeHierarchyTests
{
    private readonly Account _child = FakeAccounts.NewAccount();
    
    [Fact]
    public void Invalid_WhenAddingAccountAsItsOwnChild()
    {
        var node = GetNode();
        
        var result = node.AddChild(node.Account);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.AddAccountToItself());
    }

    [Fact]
    public void Invalid_WhenParentIsNotAPlaceholderAccount()
    {
        var node = GetNode(o => o.IsPlaceholder = false);

        var result = node.AddChild(_child);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.NoChildrenWhenNotPlaceholder());
    }

    [Fact]
    public void Invalid_WhenParentAndChildAccountTypes_AreNotTheSame()
    {
        var node = GetNode(o =>
        {
            o.Type = AccountType.Asset;
            o.IsPlaceholder = true;
        });
        var child = FakeAccounts.Get(o => o.Type = AccountType.Equity);

        var result = node.AddChild(child);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.ChildHasDifferentType(
                expected: node.Account.Type,
                actual: child.Type));
    }

    [Fact]
    public void Invalid_WhenChildAlreadyExists()
    {
        var node = GetNode();
        node.AddChild(_child);

        var result = node.AddChild(_child);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(ChartOfAccountsErrors.AccountAlreadyExists(_child));
    }

    [Fact]
    public void Parent_ShouldHaveNewChild_InChildren()
    {
        var node = GetNode();

        var result = node.AddChild(_child);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        node.Children.ShouldHaveSingleItem()
            .Account.ShouldBeEquivalentTo(_child);
    }

    [Fact]
    public void Child_ShouldHaveParent()
    {
        var node = GetNode();

        var result = node.AddChild(_child);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        var child = result.Value;
        child.Parent.ShouldBeEquivalentTo(node);
        child.ParentId.ShouldBeEquivalentTo(node.Id);
    }

    private AccountNode GetNode(Action<FakeAccountOptions>? configure = null) => AccountNode.Create(
        Guid.NewGuid(), 
        FakeAccounts.Get(configure ?? (o =>
        {
            o.IsPlaceholder = true;
            o.Type = _child.Type;
        })));
}