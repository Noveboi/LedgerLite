using Ardalis.Result;
using LedgerLite.Accounting.Core.Application;
using LedgerLite.Accounting.Core.Application.Accounts;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Tests.Unit.Utilities;

namespace LedgerLite.Accounting.Tests.Unit.UseCases;

public class CreateAccountTests
{
    private readonly Guid _chartId = Guid.NewGuid();
    private readonly AccountService _sut;
    private readonly IAccountingUnitOfWork _unitOfWork = Substitute.For<IAccountingUnitOfWork>();
    private readonly IAccountRepository _accountRepository = Substitute.For<IAccountRepository>();
    private readonly IChartOfAccountsRepository _chartRepository = Substitute.For<IChartOfAccountsRepository>();
    
    public CreateAccountTests()
    {
        _unitOfWork.AccountRepository.Returns(_accountRepository);
        _unitOfWork.ChartOfAccountsRepository.Returns(_chartRepository);
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());
        _sut = new AccountService(_unitOfWork);
    }

    [Fact]
    public async Task NoAction_WhenAccountCreateRequestIsInvalid()
    {
        var request = GetRequest(req => req with { Name = "" });
        
        var result = await _sut.CreateAccountAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ValidationErrors.ShouldHaveSingleItem().ShouldBeEquivalentTo(AccountErrors.AccountNameIsEmpty());
        await _chartRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        _accountRepository.DidNotReceive().Add(Arg.Any<Account>());
    }

    [Fact]
    public async Task NotFound_WhenChartOfAccountDoesNotExist()
    {
        var request = GetRequest();

        var result = await _sut.CreateAccountAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.NotFound);
        await _chartRepository.Received(1).GetByIdAsync(_chartId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EnsureProperDependencyCalls()
    {
        var request = GetRequest();
        var chart = FakeChartOfAccounts.Empty;
        ConfigureChart(chart);

        var result = await _sut.CreateAccountAsync(request, CancellationToken.None);
        var account = result.Value;
        
        result.Status.ShouldBe(ResultStatus.Ok);
        await _chartRepository.Received(1).GetByIdAsync(_chartId, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        _accountRepository.Received(1).Add(Arg.Is(account));
    }

    [Fact]
    public async Task AddToChart()
    {
        var request = GetRequest();
        var chart = FakeChartOfAccounts.Empty;
        ConfigureChart(chart);

        var result = await _sut.CreateAccountAsync(request, CancellationToken.None);
        var account = result.Value;
        
        result.Status.ShouldBe(ResultStatus.Ok);
        chart.Accounts.ShouldHaveSingleItem().ShouldBeEquivalentTo(account);
    }

    [Fact]
    public async Task AddToChart_WithNoParent_WhenParentIdNullInRequest()
    {
        var request = GetRequest(req => req with { ParentId = null });
        var chart = FakeChartOfAccounts.Empty;
        ConfigureChart(chart);

        var result = await _sut.CreateAccountAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        chart.Nodes.ShouldHaveSingleItem().Parent.ShouldBeNull();
    }

    [Fact]
    public async Task AddToChart_WithParent_WhenParentIdIsSpecified()
    {
        var parent = FakeAccounts.GetPlaceholder(o => o.Type = AccountType.Expense);
        var request = GetRequest(req => req with { ParentId = parent.Id });
        var chart = FakeChartOfAccounts.With(parent);
        ConfigureChart(chart);

        var result = await _sut.CreateAccountAsync(request, CancellationToken.None);
        var account = result.Value;
        
        result.Status.ShouldBe(ResultStatus.Ok);
        chart.Nodes.Count.ShouldBe(2);
        var parentNode = chart.Nodes.First(x => x.Account == parent);
        var accountNode = chart.Nodes.First(x => x.Account == account);
        parentNode.Children.ShouldHaveSingleItem().ShouldBeEquivalentTo(accountNode);
        accountNode.Parent.ShouldNotBeNull().ShouldBeEquivalentTo(parentNode);
    }
    
    private void ConfigureChart(ChartOfAccounts chart) => _chartRepository
        .GetByIdAsync(_chartId, Arg.Any<CancellationToken>())
        .Returns(chart);

    private CreateAccountRequest GetRequest(Func<CreateAccountRequest, CreateAccountRequest>? transform = null)
    {
        var request = new CreateAccountRequest(
            Name: "Groceries",
            Number: "301",
            Type: AccountType.Expense,
            Currency: Currency.Euro,
            IsPlaceholder: false,
            Description: "Tracking grocery spending.",
            ChartOfAccountsId: _chartId,
            ParentId: null);

        return transform?.Invoke(request) ?? request;
    }
}