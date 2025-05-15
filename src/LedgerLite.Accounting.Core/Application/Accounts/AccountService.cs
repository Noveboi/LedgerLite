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

    // Removing an account means that JournalEntryLines connect to the account will also be removed. Since
    // JournalEntryLines are removed, this means that JournalEntries will also need to be removed.
    //
    // Generally speaking, this use case is very delicate and needs to be handled with care.
    public Task<Result<Account>> RemoveAsync(RemoveAccountRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> MoveAsync(MoveAccountRequest request, CancellationToken token) =>
        await GetAccountByIdAsync(request.AccountId, token)
            .BindAsync(account => PositionAccountInChart(account, request.ParentId, request.Chart))
            .BindAsync(_ => unitOfWork.SaveChangesAsync(token));

    private static Result<Account> CreateAccount(CreateAccountRequest request) => Account.Create(
        name: request.Name,
        number: request.Number,
        type: request.Type,
        currency: request.Currency,
        isPlaceholder: request.IsPlaceholder,
        description: request.Description);

    private async Task<Result<Account>> GetAccountByIdAsync(Guid accountId, CancellationToken token) =>
        await unitOfWork.AccountRepository.GetByIdAsync(accountId, token) is not { } account
            ? Result.NotFound(CommonErrors.NotFound<Account>(accountId))
            : Result.Success(account);
    private static Result<Account> AddAccountToChart(Account account, ChartOfAccounts chart) => 
        chart.Add(account).Map(_ => account);

    private static Result<Account> PositionAccountInChart(Account account, Guid? parentId, ChartOfAccounts chart) =>
        parentId.HasValue
            ? chart
                .Move(accountId: account.Id, parentId: parentId.Value)
                .Map(_ => account)
            : Result.Success(account);
    
    private Result<Account> AddAccountToRepository(Account account)
    {
        unitOfWork.AccountRepository.Add(account);
        return account;
    }
}