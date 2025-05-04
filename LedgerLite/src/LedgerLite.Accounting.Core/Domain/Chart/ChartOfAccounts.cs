using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.Chart;

/// <summary>
/// Organizes all <see cref="Account"/>s that are used for a fiscal period.
/// </summary>
public sealed class ChartOfAccounts : AuditableEntity
{
    /*
     * Accounts are organized hierarchically, instead of having the organization logic and data structures in the Account
     * entity, we delegate the responsibility to the ChartOfAccounts entity which is solely responsible for the organization
     * of accounts. The way the hierarchy is modelled here is with the use of an Adjacency List, done in a way to also
     * be compatible with EF Core and relational databases.
     */
    
    private ChartOfAccounts() { }
    
    private readonly List<AccountNode> _nodes = [];
    public IEnumerable<Account> Accounts => _nodes.Select(x => x.Account);

    public static Result<ChartOfAccounts> Create() => Result.Success(new ChartOfAccounts());

    /// <summary>
    /// Creates an account at the 'root' level of the hierarchy.
    /// </summary>
    public Result<ChartOfAccounts> Add(Account account)
    {
        if (_nodes.Any(acc => acc == account))
            return Result.Conflict($"Account {account} already exists.");

        var node = AccountNode.Create(Id, account);
        _nodes.Add(node);
        
        return this;
    }
    
    /// <summary>
    /// Establishes a parent-child relationship between two accounts.
    /// </summary>
    public Result<ChartOfAccounts> Attach(Account account, Guid parentId)
    {
        var parent = _nodes.FirstOrDefault(x => x.Account.Id == parentId);
        if (parent is null)
            return Result.NotFound($"Couldn't find account with ID {parentId}");

        var addChildResult = parent.AddChild(account);
        if (!addChildResult.IsOk())
            return addChildResult.Map();

        var node = addChildResult.Value;
        _nodes.Add(node);
        
        return this;
    }
}