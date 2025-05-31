using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.Domain.Chart;

public class ChartOfAccountsMoveTests
{
    [Fact]
    public void ChangeParent_OfMoveSubject_WhenNoParent()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var chart = FakeChartOfAccounts.With(subject, parent);

        var result = chart.Move(accountId: subject.Id, parentId: parent.Id);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        var subjectNode = GetNodeFromChart(subject: subject, chart: chart);
        var parentNode = GetNodeFromChart(subject: parent, chart: chart);
        subjectNode.Parent.ShouldNotBeNull().Account.ShouldBeEquivalentTo(expected: parent);
        subjectNode.ParentId.ShouldNotBeNull().ShouldBe(expected: parentNode.Id);
    }

    [Fact]
    public void RemoveMoveSubject_FromChildrenOfPreviousParent()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var oldParent = FakeAccounts.Get(configure: o =>
        {
            o.Type = subject.Type;
            o.IsPlaceholder = true;
        });
        var chart = FakeChartOfAccounts.With(parent, (oldParent, o => o.AddChild(child: subject)));

        var result = chart.Move(accountId: subject.Id, parentId: parent.Id);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        var oldParentNode = GetNodeFromChart(subject: oldParent, chart: chart);
        oldParentNode.Children.ShouldBeEmpty();
    }

    [Fact]
    public void AddMoveSubject_ToChildrenOfNextParent()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var chart = FakeChartOfAccounts.With(subject, parent);

        var result = chart.Move(accountId: subject.Id, parentId: parent.Id);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        var parentNode = GetNodeFromChart(subject: parent, chart: chart);
        parentNode.Children.ShouldHaveSingleItem().Account.ShouldBeEquivalentTo(expected: subject);
    }

    [Fact]
    public void Invalid_WhenMoveSubject_DoesNotExist()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var chart = FakeChartOfAccounts.With(parent);

        var result = chart.Move(accountId: subject.Id, parentId: parent.Id);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: ChartOfAccountsErrors.AccountNotFound(id: subject.Id));
    }

    [Fact]
    public void Invalid_WhenParent_DoesNotExist()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var chart = FakeChartOfAccounts.With(subject);

        var result = chart.Move(accountId: subject.Id, parentId: parent.Id);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: ChartOfAccountsErrors.AccountNotFound(id: parent.Id));
    }

    [Fact]
    public void Invalid_WhenParent_NotPlaceholder()
    {
        var (subject, parent) = GetSubjectAndTargetParent(configureParent: o => o.IsPlaceholder = false);
        var chart = FakeChartOfAccounts.With(subject, parent);

        var result = chart.Move(accountId: subject.Id, parentId: parent.Id);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: AccountErrors.NoChildrenWhenNotPlaceholder(account: parent));
    }

    [Fact]
    public void Invalid_WhenChild_NotSameAccountType_AsParent()
    {
        var subject = FakeAccounts.Get(configure: o => o.Type = AccountType.Asset);
        var parent = FakeAccounts.GetPlaceholder(configure: o => o.Type = AccountType.Equity);
        var chart = FakeChartOfAccounts.With(subject, parent);

        var result = chart.Move(accountId: subject.Id, parentId: parent.Id);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(
                expected: AccountErrors.ChildHasDifferentType(expected: parent.Type, actual: subject.Type));
    }

    [Fact]
    public void Invalid_WhenMovingToSameParent()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var chart = FakeChartOfAccounts.With((parent, o => o.AddChild(child: subject)));

        var result = chart.Move(accountId: subject.Id, parentId: parent.Id);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: ChartOfAccountsErrors.MoveToSameParent());
    }

    private static (Account, Account) GetSubjectAndTargetParent(
        Action<FakeAccountOptions>? configureSubject = null,
        Action<FakeAccountOptions>? configureParent = null)
    {
        var parent = FakeAccounts.Get(configure: configureParent ?? (x => x.IsPlaceholder = true));
        var subject = FakeAccounts.Get(configure: configureSubject ?? (x => x.Type = parent.Type));

        return (subject, parent);
    }

    private static AccountNode GetNodeFromChart(Account subject, ChartOfAccounts chart)
    {
        chart.Nodes.ShouldContain(elementPredicate: node => node.Account == subject);
        var node = chart.Nodes.First(predicate: x => x.Account == subject);

        return node;
    }
}