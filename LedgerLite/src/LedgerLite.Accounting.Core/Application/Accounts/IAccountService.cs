using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Application.Accounts;

public interface IAccountService
{
    Task<Result<Account>> CreateAccountAsync(CreateAccountRequest request, CancellationToken token);
    Task<Result<Account>> RemoveAccountAsync(RemoveAccountRequest request, CancellationToken token);
    Task<Result> MoveAccountAsync(MoveAccountRequest request, CancellationToken token);
}