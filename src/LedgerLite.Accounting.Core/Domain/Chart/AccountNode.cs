using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.Chart;

public sealed class AccountNode : Entity
{
    private readonly List<AccountNode> _children = [];

    private AccountNode()
    {
    }

    private AccountNode(Guid chartId, Account account)
    {
        ChartId = chartId;
        Account = account;
        AccountId = account.Id;
    }

    /// <summary>
    ///     The <see cref="ChartOfAccounts" /> which contains this node.
    /// </summary>
    public Guid ChartId { get; private init; }

    /// <summary>
    ///     The underlying account that is wrapped by the node.
    /// </summary>
    public Account Account { get; } = null!;

    public Guid AccountId { get; private set; }

    public Guid? ParentId { get; private set; }
    public AccountNode? Parent { get; private set; }
    public IReadOnlyCollection<AccountNode> Children => _children;

    public static AccountNode Create(Guid chartId, Account account)
    {
        return new AccountNode(chartId: chartId, account: account);
    }

    public Result AddChild(AccountNode child)
    {
        if (Account == child.Account)
            return Result.Invalid(validationError: AccountErrors.AddAccountToItself());

        if (!Account.IsPlaceholder)
            return Result.Invalid(validationError: AccountErrors.NoChildrenWhenNotPlaceholder(account: Account));

        if (Account.Type != child.Account.Type)
            return Result.Invalid(validationError: AccountErrors.ChildHasDifferentType(
                expected: Account.Type,
                actual: child.Account.Type));

        if (_children.Any(predicate: node => node.Account == child.Account))
            return Result.Invalid(
                validationError: ChartOfAccountsErrors.AccountAlreadyExists(existingAccount: child.Account));

        _children.Add(item: child);
        child.Parent = this;
        child.ParentId = Id;

        return Result.Success();
    }

    public Result RemoveChild(AccountNode child)
    {
        if (_children.Count == 0)
            return Result.Invalid(
                validationError: ChartOfAccountsErrors.AccountHasNoChildrenToRemove(account: Account));

        if (!_children.Remove(item: child))
            return Result.Invalid(
                validationError: ChartOfAccountsErrors.AccountNotChild(parent: Account, child: child.Account));

        child.Parent = null;
        child.ParentId = null!;

        return Result.Success();
    }

    public override string ToString()
    {
        return $"{Account} ({_children.Count} children, {(Parent is null ? "Root" : "Leaf")})";
    }
}