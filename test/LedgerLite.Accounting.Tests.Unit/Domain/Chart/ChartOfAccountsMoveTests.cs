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

        var result = chart.Move(subject.Id, parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        var subjectNode = GetNodeFromChart(subject, chart);
        var parentNode = GetNodeFromChart(parent, chart);
        subjectNode.Parent.ShouldNotBeNull().Account.ShouldBeEquivalentTo(parent);
        subjectNode.ParentId.ShouldNotBeNull().ShouldBe(parentNode.Id);
    }
    
    [Fact]
    public void RemoveMoveSubject_FromChildrenOfPreviousParent()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var oldParent = FakeAccounts.Get(o =>
        {
            o.Type = subject.Type;
            o.IsPlaceholder = true;
        });
        var chart = FakeChartOfAccounts.With(parent, (oldParent, o => o.AddChild(subject)));

        var result = chart.Move(subject.Id, parent.Id);

        result.Status.ShouldBe(ResultStatus.Ok);
        var oldParentNode = GetNodeFromChart(oldParent, chart);
        oldParentNode.Children.ShouldBeEmpty();
    }

    [Fact]
    public void AddMoveSubject_ToChildrenOfNextParent()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var chart = FakeChartOfAccounts.With(subject, parent);

        var result = chart.Move(subject.Id, parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        var parentNode = GetNodeFromChart(parent, chart);
        parentNode.Children.ShouldHaveSingleItem().Account.ShouldBeEquivalentTo(subject);
    }

    [Fact]
    public void Invalid_WhenMoveSubject_DoesNotExist()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var chart = FakeChartOfAccounts.With(parent);

        var result = chart.Move(subject.Id, parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(ChartOfAccountsErrors.AccountNotFound(subject.Id));
    }

    [Fact]
    public void Invalid_WhenParent_DoesNotExist()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var chart = FakeChartOfAccounts.With(subject);

        var result = chart.Move(subject.Id, parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(ChartOfAccountsErrors.AccountNotFound(parent.Id));
    }

    [Fact]
    public void Invalid_WhenParent_NotPlaceholder()
    {
        var (subject, parent) = GetSubjectAndTargetParent(configureParent: o => o.IsPlaceholder = false);
        var chart = FakeChartOfAccounts.With(subject, parent);

        var result = chart.Move(subject.Id, parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.NoChildrenWhenNotPlaceholder(parent));
    }

    [Fact]
    public void Invalid_WhenChild_NotSameAccountType_AsParent()
    {
        var subject = FakeAccounts.Get(o => o.Type = AccountType.Asset);
        var parent = FakeAccounts.GetPlaceholder(o => o.Type = AccountType.Equity);
        var chart = FakeChartOfAccounts.With(subject, parent);

        var result = chart.Move(subject.Id, parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(AccountErrors.ChildHasDifferentType(parent.Type, subject.Type));
    }

    [Fact]
    public void Invalid_WhenMovingToSameParent()
    {
        var (subject, parent) = GetSubjectAndTargetParent();
        var chart = FakeChartOfAccounts.With((parent, o => o.AddChild(subject)));

        var result = chart.Move(subject.Id, parent.Id);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors
            .ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(ChartOfAccountsErrors.MoveToSameParent());
    }

    private static (Account, Account) GetSubjectAndTargetParent(
        Action<FakeAccountOptions>? configureSubject = null,
        Action<FakeAccountOptions>? configureParent = null)
    {
        var parent = FakeAccounts.Get(configureParent ?? (x => x.IsPlaceholder = true));
        var subject = FakeAccounts.Get(configureSubject ?? (x => x.Type = parent.Type));

        return (subject, parent);
    }

    private static AccountNode GetNodeFromChart(Account subject, ChartOfAccounts chart)
    {
        chart.Nodes.ShouldContain(node => node.Account == subject);
        var node = chart.Nodes.First(x => x.Account == subject);

        return node;
    }
}