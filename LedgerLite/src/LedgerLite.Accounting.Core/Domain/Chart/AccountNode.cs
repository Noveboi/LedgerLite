using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.Chart;

public sealed class AccountNode : Entity
{
    private AccountNode() {}
    private AccountNode(Guid chartId)
    {
        ChartId = chartId;
    }
    
    /// <summary>
    /// The <see cref="ChartOfAccounts"/> which contains this node.
    /// </summary>
    public Guid ChartId { get; private init; }
    
    /// <summary>
    /// The underlying account that is wrapped by the node.
    /// </summary>
    public Account Account { get; private set; } = null!;
    
    public Guid? ParentId { get; private set; }
    public AccountNode? Parent { get; private set; }

    private readonly List<AccountNode> _children = [];
    public IReadOnlyCollection<AccountNode> Children => _children;
    
    public static AccountNode Create(Guid chartId, Account account) => new(chartId)
    {
        Account = account
    };

    private static AccountNode CreateAttachedTo(AccountNode parent, Account account, Guid chartId)
    {
        var node = new AccountNode(chartId)
        {
            Account = account,
            Parent = parent,
            ParentId = parent.Id
        };

        return node;
    }

    public Result<AccountNode> AddChild(Account child)
    {
        if (Account == child)
            return Result.Invalid(AccountErrors.AddAccountToItself());

        if (!Account.IsPlaceholder)
            return Result.Invalid(AccountErrors.NoChildrenWhenNotPlaceholder());

        if (Account.Type != child.Type)
            return Result.Invalid(AccountErrors.ChildHasDifferentType(
                expected: Account.Type, 
                actual: child.Type));
        
        if (_children.Any(node => node.Account == child))
            return Result.Invalid(ChartOfAccountsErrors.AccountAlreadyExists(child));

        var childNode = CreateAttachedTo(this, child, ChartId);
        _children.Add(childNode);

        return childNode;
    }
}