using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.SharedKernel.Domain.Errors;

namespace LedgerLite.Accounting.Core.Application.Accounts;

internal sealed class AccountService(IAccountingUnitOfWork unitOfWork) : IAccountService
{
    public async Task<Result<Account>> CreateAsync(CreateAccountRequest request, CancellationToken token)
    {
        var createAccountResult = CreateAccount(request: request);
        if (!createAccountResult.IsSuccess)
            return createAccountResult;

        var account = createAccountResult.Value;
        var chart = request.Chart;

        return await AddAccountToChart(account: account, chart: chart)
            .Bind(bindFunc: acc => PositionAccountInChart(account: acc, parentId: request.ParentId, chart: chart))
            .Bind(bindFunc: AddAccountToRepository)
            .BindAsync(bindFunc: acc => unitOfWork.SaveChangesAsync(token: token).MapAsync(func: () => acc));
    }

    public async Task<Result> MoveAsync(MoveAccountRequest request, CancellationToken token)
    {
        return await GetByIdAsync(accountId: request.AccountId, token: token)
            .BindAsync(bindFunc: account =>
                PositionAccountInChart(account: account, parentId: request.ParentId, chart: request.Chart))
            .BindAsync(bindFunc: _ => unitOfWork.SaveChangesAsync(token: token));
    }

    public async Task<Result<Account>> GetByIdAsync(Guid accountId, CancellationToken token)
    {
        return await unitOfWork.AccountRepository.GetByIdAsync(id: accountId, token: token) is not { } account
            ? Result.NotFound(CommonErrors.NotFound<Account>(id: accountId))
            : Result.Success(value: account);
    }

    private static Result<Account> CreateAccount(CreateAccountRequest request)
    {
        return Account.Create(
            name: request.Name,
            number: request.Number,
            type: request.Type,
            currency: request.Currency,
            isPlaceholder: request.IsPlaceholder,
            description: request.Description);
    }

    private static Result<Account> AddAccountToChart(Account account, ChartOfAccounts chart)
    {
        return chart.Add(account: account).Map(func: _ => account);
    }

    private static Result<Account> PositionAccountInChart(Account account, Guid? parentId, ChartOfAccounts chart)
    {
        return parentId.HasValue
            ? chart
                .Move(accountId: account.Id, parentId: parentId.Value)
                .Map(func: _ => account)
            : Result.Success(value: account);
    }

    private Result<Account> AddAccountToRepository(Account account)
    {
        unitOfWork.AccountRepository.Add(account: account);
        return account;
    }
}