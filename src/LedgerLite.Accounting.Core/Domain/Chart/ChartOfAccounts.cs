using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Accounts.Metadata;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.Chart;

/// <summary>
///     Organizes all <see cref="Account" />s that are used for an organization.
/// </summary>
public sealed class ChartOfAccounts : AuditableEntity
{
    private readonly List<AccountNode> _nodes = [];
    /*
     * Accounts are organized hierarchically, instead of having the organization logic and data structures in the Account
     * entity, we delegate the responsibility to the ChartOfAccounts entity which is solely responsible for the organization
     * of accounts. The way the hierarchy is modelled here is with the use of an Adjacency List, done in a way to also
     * be compatible with EF Core and relational databases.
     */

    private ChartOfAccounts()
    {
    }

    private ChartOfAccounts(Guid organizationId)
    {
        OrganizationId = organizationId;
    }

    public IReadOnlyCollection<Account> Accounts => _nodes.Select(x => x.Account).ToList();
    public IReadOnlyCollection<AccountNode> Nodes => _nodes;

    public Guid OrganizationId { get; }

    public static Result<ChartOfAccounts> Create(Guid organizationId)
    {
        var chart = new ChartOfAccounts(organizationId: organizationId);
        return Result.Success(value: chart);
    }

    /// <summary>
    ///     Creates an account initially at the 'root' level of the hierarchy.
    /// </summary>
    public Result Add(Account account)
    {
        if (_nodes.Any(acc => acc.Account == account))
            return Result.Invalid(
                ChartOfAccountsErrors.AccountAlreadyExists(existingAccount: account));

        var node = AccountNode.Create(chartId: Id, account: account);
        _nodes.Add(item: node);

        return Result.Success();
    }

    /// <summary>
    ///     Position the target account under the desired parent.
    /// </summary>
    public Result Move(Guid accountId, Guid parentId)
    {
        var account = _nodes.FirstOrDefault(node => node.Account.Id == accountId);
        if (account is null)
            return Result.Invalid(ChartOfAccountsErrors.AccountNotFound(id: accountId));

        var parent = _nodes.FirstOrDefault(node => node.Account.Id == parentId);
        if (parent is null)
            return Result.Invalid(ChartOfAccountsErrors.AccountNotFound(id: parentId));

        if (account.Parent == parent)
            return Result.Invalid(ChartOfAccountsErrors.MoveToSameParent());

        var removeChildResult = account.Parent?.RemoveChild(child: account);
        if (removeChildResult is { IsSuccess: false })
            return removeChildResult.Map();

        if (parent.Account.Type == AccountType.Expense &&
            parent.Account.Metadata.ExpenseType is var parentExpense and not ExpenseType.Undefined)
        {
            if (account.Account.Metadata.ExpenseType is var expense and not ExpenseType.Undefined && expense != parentExpense)
            {
                return Result.Invalid(ChartOfAccountsErrors.AccountInvalidExpenseType(
                    parent: parent.Account,
                    target: account.Account));
            }
            
            account.Account.Metadata = account.Account.Metadata with { ExpenseType = parentExpense };
        }

        var addChildResult = parent.AddChild(child: account);
        if (!addChildResult.IsSuccess)
            return addChildResult;

        return Result.Success();
    }
}