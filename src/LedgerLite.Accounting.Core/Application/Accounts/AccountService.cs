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
        var createAccountResult = CreateAccount(request);
        if (!createAccountResult.IsSuccess)
            return createAccountResult;

        var account = createAccountResult.Value;
        var chart = request.Chart;

        return await AddAccountToChart(account, chart)
            .Bind(acc => PositionAccountInChart(acc, request.ParentId, chart))
            .Bind(AddAccountToRepository)
            .BindAsync(acc => unitOfWork.SaveChangesAsync(token).MapAsync(() => acc));
    }

    public async Task<Result> MoveAsync(MoveAccountRequest request, CancellationToken token)
    {
        return await GetByIdAsync(request.AccountId, token)
            .BindAsync(account => PositionAccountInChart(account, request.ParentId, request.Chart))
            .BindAsync(_ => unitOfWork.SaveChangesAsync(token));
    }

    public async Task<Result<Account>> GetByIdAsync(Guid accountId, CancellationToken token)
    {
        return await unitOfWork.AccountRepository.GetByIdAsync(accountId, token) is not { } account
            ? Result.NotFound(CommonErrors.NotFound<Account>(accountId))
            : Result.Success(account);
    }

    private static Result<Account> CreateAccount(CreateAccountRequest request)
    {
        return Account.Create(
            request.Name,
            request.Number,
            request.Type,
            request.Currency,
            request.IsPlaceholder,
            request.Description);
    }

    private static Result<Account> AddAccountToChart(Account account, ChartOfAccounts chart)
    {
        return chart.Add(account).Map(_ => account);
    }

    private static Result<Account> PositionAccountInChart(Account account, Guid? parentId, ChartOfAccounts chart)
    {
        return parentId.HasValue
            ? chart
                .Move(account.Id, parentId.Value)
                .Map(_ => account)
            : Result.Success(account);
    }

    private Result<Account> AddAccountToRepository(Account account)
    {
        unitOfWork.AccountRepository.Add(account);
        return account;
    }
}