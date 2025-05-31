using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class AccountNodeAddChildTests
{
    [Fact]
    public void Invalid_WhenAddingAccountAsItsOwnChild()
    {
        var node = FakeAccountNodes.Get();

        var result = node.AddChild(child: node);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: AccountErrors.AddAccountToItself());
    }

    [Fact]
    public void Invalid_WhenParentIsNotAPlaceholderAccount()
    {
        var node = FakeAccountNodes.Get(configure: o => o.IsPlaceholder = false);

        var result = node.AddChild(child: FakeAccountNodes.SampleChild);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: AccountErrors.NoChildrenWhenNotPlaceholder(account: node.Account));
    }

    [Fact]
    public void Invalid_WhenParentAndChildAccountTypes_AreNotTheSame()
    {
        var node = FakeAccountNodes.Get(configure: o =>
        {
            o.Type = AccountType.Asset;
            o.IsPlaceholder = true;
        });
        var child = FakeAccountNodes.Get(configure: o => o.Type = AccountType.Equity);

        var result = node.AddChild(child: child);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: AccountErrors.ChildHasDifferentType(
                expected: node.Account.Type,
                actual: child.Account.Type));
    }

    [Fact]
    public void Invalid_WhenChildAlreadyExists()
    {
        var node = FakeAccountNodes.Get();
        node.AddChild(child: FakeAccountNodes.SampleChild);

        var result = node.AddChild(child: FakeAccountNodes.SampleChild);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(
                expected: ChartOfAccountsErrors.AccountAlreadyExists(
                    existingAccount: FakeAccountNodes.SampleChild.Account));
    }

    [Fact]
    public void Parent_ShouldHaveNewChild_InChildren()
    {
        var node = FakeAccountNodes.Get();

        var result = node.AddChild(child: FakeAccountNodes.SampleChild);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        node.Children
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: FakeAccountNodes.SampleChild);
    }

    [Fact]
    public void Child_ShouldHaveParent()
    {
        var node = FakeAccountNodes.Get();
        var child = FakeAccountNodes.SampleChild;

        var result = node.AddChild(child: child);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        child.Parent.ShouldBeEquivalentTo(expected: node);
        child.ParentId.ShouldBeEquivalentTo(expected: node.Id);
    }
}