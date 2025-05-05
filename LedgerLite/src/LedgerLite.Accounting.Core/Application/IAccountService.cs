using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;

namespace LedgerLite.Accounting.Core.Application;

public interface IAccountService
{
    Task<Result<Account>> CreateAccountAsync(CreateAccountRequest request, CancellationToken token);
    Task<Result<Account>> RemoveAccountAsync(RemoveAccountRequest request, CancellationToken token);
    Task<Result> MoveAccountAsync(MoveAccountRequest request, CancellationToken token);
}

public sealed record CreateAccountRequest(
    string Name,
    string Number,
    AccountType Type,
    Currency Currency,
    bool IsPlaceholder,
    string Description,
    Guid ChartOfAccountsId,
    Guid? ParentId);
    
public sealed record RemoveAccountRequest(Guid AccountId);
public sealed record MoveAccountRequest(Guid AccountId, Guid ParentId);