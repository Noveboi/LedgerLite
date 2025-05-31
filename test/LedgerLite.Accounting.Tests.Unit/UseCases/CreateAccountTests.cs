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
        _unitOfWork.ConfigureForTests(o => o
            .MockAccountRepository(_accountRepository));
        _sut = new AccountService(_unitOfWork);
    }

    [Fact]
    public async Task NoAction_WhenAccountCreateRequestIsInvalid()
    {
        var request = GetRequest(req => req with { Name = "" });

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors.ShouldHaveSingleItem().ShouldBeEquivalentTo(AccountErrors.AccountNameIsEmpty());
        await _unitOfWork.AssertThatNoActionWasTaken();
    }

    [Fact]
    public async Task EnsureProperDependencyCalls()
    {
        var request = GetRequest();

        var result = await _sut.CreateAsync(request, CancellationToken.None);
        var account = result.Value;

        result.Status.ShouldBe(ResultStatus.Ok);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        _accountRepository.Received(1).Add(Arg.Is(account));
    }

    [Fact]
    public async Task AddToChart()
    {
        var request = GetRequest();

        var result = await _sut.CreateAsync(request, CancellationToken.None);
        var account = result.Value;

        result.Status.ShouldBe(ResultStatus.Ok);
        _chart.Accounts.ShouldHaveSingleItem().ShouldBeEquivalentTo(account);
    }

    [Fact]
    public async Task AddToChart_WithNoParent_WhenParentIdNullInRequest()
    {
        var request = GetRequest(req => req with { ParentId = null });

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Ok);
        _chart.Nodes.ShouldHaveSingleItem().Parent.ShouldBeNull();
    }

    [Fact]
    public async Task AddToChart_WithParent_WhenParentIdIsSpecified()
    {
        var parent = FakeAccounts.GetPlaceholder(o => o.Type = AccountType.Expense);
        var chart = FakeChartOfAccounts.With(parent);
        var request = GetRequest(req => req with { ParentId = parent.Id, Chart = chart });

        var result = await _sut.CreateAsync(request, CancellationToken.None);
        var account = result.Value;

        result.Status.ShouldBe(ResultStatus.Ok);
        chart.Nodes.Count.ShouldBe(2);
        var parentNode = chart.Nodes.First(x => x.Account == parent);
        var accountNode = chart.Nodes.First(x => x.Account == account);
        parentNode.Children.ShouldHaveSingleItem().ShouldBeEquivalentTo(accountNode);
        accountNode.Parent.ShouldNotBeNull().ShouldBeEquivalentTo(parentNode);
    }

    private CreateAccountRequest GetRequest(Func<CreateAccountRequest, CreateAccountRequest>? transform = null)
    {
        var request = new CreateAccountRequest(
            "Groceries",
            "301",
            AccountType.Expense,
            Currency.Euro,
            false,
            "Tracking grocery spending.",
            _chart,
            null);

        return transform?.Invoke(request) ?? request;
    }
}