using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.Chart;

/// <summary>
/// Organizes all <see cref="Account"/>s that are used for a fiscal period.
/// </summary>
public sealed class ChartOfAccounts : AuditableEntity
{
    private readonly List<AccountNode> _accounts = [];
    
    private ChartOfAccounts() { }

    // Adjacency list of accounts and their neighbouring accounts.
    public IReadOnlyCollection<AccountNode> Accounts => _accounts;

    public static ChartOfAccounts Create() => new();
    public Result AddRootAccount(Account account)
    {
        if (_accounts.Any(acc => acc == account))
            return Result.Conflict($"Account {account} already exists.");

        var node = AccountNode.CreateRoot(Id, account);
        _accounts.Add(node);
        return Result.Success();
    }
    
    public Result AddAccountWithParent(Account account, Account parent)
    {
        if (_accounts.Any(acc => acc == account))
            return Result.Conflict($"Account {account} already exists.");
        
        if (account == parent)
            return Result.Invalid(AccountErrors.AddAccountToItself());

        if (!parent.IsPlaceholder)
            return Result.Invalid(AccountErrors.NoChildrenWhenNotPlaceholder());

        if (account.Type != parent.Type)
            return Result.Invalid(AccountErrors.ChildHasDifferentType(
                expected: parent.Type, 
                actual: account.Type));

        var node = AccountNode.CreateWithParent(
            chartId: Id,
            account: account,
            parent: parent);
        
        _accounts.Add(node);
        
        return Result.Success();
    }
}