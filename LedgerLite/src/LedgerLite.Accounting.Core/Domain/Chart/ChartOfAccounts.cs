using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.Chart;

/// <summary>
/// Organizes all <see cref="Account"/>s that are used for a fiscal period.
/// </summary>
public sealed class ChartOfAccounts : AuditableEntity
{
    private readonly List<Account> _accounts = [];
    
    private ChartOfAccounts() { }

    public Guid UserId { get; private init; }
    
    /// <summary>
    /// All root-level accounts.
    /// </summary>
    public IReadOnlyCollection<Account> Accounts => _accounts;

    public static ChartOfAccounts Create(Guid userId) => new()
    {
        UserId = userId
    };

    public Result AddAccount(Account account)
    {
        if (!account.IsRootLevel)
            return Result.Invalid(ChartOfAccountsErrors.AccountNotRootLevel());
        
        _accounts.Add(account);
        return Result.Success();
    }

    public Result RemoveAccount(Account account) =>
        _accounts.Remove(account) 
            ? Result.Success() 
            : Result.NotFound($"Account {account} does not exist in current Chart of Accounts.");
}