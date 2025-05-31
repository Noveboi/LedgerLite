using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.Accounting.Tests.Unit.Utilities;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

namespace LedgerLite.Accounting.Tests.Unit.UseCases;

public class CreateAccountTests
{
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly ChartOfAccounts _chart = FakeChartOfAccounts.Empty;
    private readonly AccountService _sut;
    private readonly IAccountingUnitOfWork _unitOfWork = Substitute.For<IAccountingUnitOfWork>();

    public CreateAccountTests()
    {
        _unitOfWork.ConfigureForTests(configure: o => o
            .MockAccountRepository(repo: _accountRepository));
        _sut = new AccountService(unitOfWork: _unitOfWork);
    }

    [Fact]
    public async Task NoAction_WhenAccountCreateRequestIsInvalid()
    {
        var request = GetRequest(transform: req => req with { Name = "" });

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ValidationErrors.ShouldHaveSingleItem()
            .ShouldBeEquivalentTo(expected: AccountErrors.AccountNameIsEmpty());
        await _unitOfWork.AssertThatNoActionWasTaken();
    }

    [Fact]
    public async Task EnsureProperDependencyCalls()
    {
        var request = GetRequest();

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);
        var account = result.Value;

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        await _unitOfWork.Received(requiredNumberOfCalls: 1).SaveChangesAsync(token: Arg.Any<CancellationToken>());
        _accountRepository.Received(requiredNumberOfCalls: 1).Add(account: Arg.Is(value: account));
    }

    [Fact]
    public async Task AddToChart()
    {
        var request = GetRequest();

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);
        var account = result.Value;

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        _chart.Accounts.ShouldHaveSingleItem().ShouldBeEquivalentTo(expected: account);
    }

    [Fact]
    public async Task AddToChart_WithNoParent_WhenParentIdNullInRequest()
    {
        var request = GetRequest(transform: req => req with { ParentId = null });

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        _chart.Nodes.ShouldHaveSingleItem().Parent.ShouldBeNull();
    }

    [Fact]
    public async Task AddToChart_WithParent_WhenParentIdIsSpecified()
    {
        var parent = FakeAccounts.GetPlaceholder(configure: o => o.Type = AccountType.Expense);
        var chart = FakeChartOfAccounts.With(parent);
        var request = GetRequest(transform: req => req with { ParentId = parent.Id, Chart = chart });

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);
        var account = result.Value;

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        chart.Nodes.Count.ShouldBe(expected: 2);
        var parentNode = chart.Nodes.First(predicate: x => x.Account == parent);
        var accountNode = chart.Nodes.First(predicate: x => x.Account == account);
        parentNode.Children.ShouldHaveSingleItem().ShouldBeEquivalentTo(expected: accountNode);
        accountNode.Parent.ShouldNotBeNull().ShouldBeEquivalentTo(expected: parentNode);
    }

    private CreateAccountRequest GetRequest(Func<CreateAccountRequest, CreateAccountRequest>? transform = null)
    {
        var request = new CreateAccountRequest(
            Name: "Groceries",
            Number: "301",
            Type: AccountType.Expense,
            Currency: Currency.Euro,
            IsPlaceholder: false,
            Description: "Tracking grocery spending.",
            Chart: _chart,
            ParentId: null);

        return transform?.Invoke(arg: request) ?? request;
    }
}