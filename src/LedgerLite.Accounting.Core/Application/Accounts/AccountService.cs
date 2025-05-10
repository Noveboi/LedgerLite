using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Accounting.Core.Infrastructure;

namespace LedgerLite.Accounting.Core.Application.Accounts;

internal sealed class AccountService(IAccountingUnitOfWork unitOfWork) : IAccountService
{
    public async Task<Result<Account>> CreateAsync(CreateAccountRequest request, CancellationToken token)
    {
        var createAccountResult = CreateAccount(request);
        if (!createAccountResult.IsSuccess)
            return createAccountResult;
        
        var chartResult = await GetChartOfAccountsAsync(request.ChartOfAccountsId, token);
        if (!chartResult.IsSuccess)
            return chartResult.Map();

        var account = createAccountResult.Value;
        var chart = chartResult.Value;

        return await AddAccountToChart(account, chart)
            .Bind(acc => PositionAccountInChart(acc, request.ParentId, chart))
            .Bind(acc => AddAccountToRepository(acc))
            .BindAsync(acc => SaveChangesAsync(token).MapAsync(() => acc));
    }

    public Task<Result<Account>> RemoveAsync(RemoveAccountRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<Result> MoveAsync(MoveAccountRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    private static Result<Account> CreateAccount(CreateAccountRequest request) => Account.Create(
        name: request.Name,
        number: request.Number,
        type: request.Type,
        currency: request.Currency,
        isPlaceholder: request.IsPlaceholder,
        description: request.Description);

    private async Task<Result<ChartOfAccounts>> GetChartOfAccountsAsync(Guid chartId, CancellationToken token)
    {
        var chart = await unitOfWork.ChartOfAccountsRepository.GetByIdAsync(chartId, token);
        return chart is not null 
            ? chart
            : Result.NotFound($"Chart of Accounts with ID '{chartId}' does not exist.");
    }
    
    private static Result<Account> AddAccountToChart(Account account, ChartOfAccounts chart) => 
        chart
            .Add(account)
            .Map(_ => account);

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

    private Task<Result> SaveChangesAsync(CancellationToken token) => unitOfWork.SaveChangesAsync(token);
}