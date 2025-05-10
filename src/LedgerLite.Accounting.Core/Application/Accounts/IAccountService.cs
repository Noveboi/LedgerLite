using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.Accounts.Requests;
using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Application.Accounts;

public interface IAccountService
{
    Task<Result<Account>> CreateAsync(CreateAccountRequest request, CancellationToken token);
    Task<Result<Account>> RemoveAsync(RemoveAccountRequest request, CancellationToken token);
    Task<Result> MoveAsync(MoveAccountRequest request, CancellationToken token);
}