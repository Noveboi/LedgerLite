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

    private ChartOfAccounts(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
    
    private readonly List<AccountNode> _nodes = [];
    public IReadOnlyCollection<Account> Accounts => _nodes.Select(x => x.Account).ToList();
    public IReadOnlyCollection<AccountNode> Nodes => _nodes;

    public Guid OrganizationId { get; }
    
    public static Result<ChartOfAccounts> Create(Guid organizationId)
    {
        var chart = new ChartOfAccounts(organizationId);
        return Result.Success(chart);
    }

    /// <summary>
    /// Creates an account initially at the 'root' level of the hierarchy.
    /// </summary>
    public Result Add(Account account)
    {
        if (_nodes.Any(acc => acc.Account == account))
            return Result.Invalid(ChartOfAccountsErrors.AccountAlreadyExists(account));

        var node = AccountNode.Create(Id, account);
        _nodes.Add(node);
        
        return Result.Success();
    }

    /// <summary>
    /// Position the target account under the desired parent.
    /// </summary>
    public Result Move(Guid accountId, Guid parentId)
    {
        var account = _nodes.FirstOrDefault(node => node.Account.Id == accountId);
        if (account is null)
            return Result.Invalid(ChartOfAccountsErrors.AccountNotFound(accountId));

        var parent = _nodes.FirstOrDefault(node => node.Account.Id == parentId);
        if (parent is null)
            return Result.Invalid(ChartOfAccountsErrors.AccountNotFound(parentId));

        if (account.Parent == parent)
            return Result.Invalid(ChartOfAccountsErrors.MoveToSameParent());

        var removeChildResult = account.Parent?.RemoveChild(account);
        if (removeChildResult is { IsSuccess: false })
            return removeChildResult.Map();

        var addChildResult = parent.AddChild(account);
        if (!addChildResult.IsSuccess)
            return addChildResult;

        return Result.Success();
    }
}