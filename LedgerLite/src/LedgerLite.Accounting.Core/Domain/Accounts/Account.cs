using Ardalis.Result;
using LedgerLite.SharedKernel.Domain;

namespace LedgerLite.Accounting.Core.Domain.Accounts;

public sealed class Account : AuditableEntity
{
    private Account() { }
    
    public string Number { get; private init; } = null!;
    public string Name { get; private init; } = null!;
    public string Description { get; private init; } = "";

    public AccountType Type { get; private init; } = null!;
    public Currency Currency { get; private init; } = null!;
    public bool IsPlaceholder { get; private init; }
    
    public int HierarchyLevel { get; private set; }
    public Guid? ParentAccountId { get; private set; }

    private readonly List<Account> _childAccounts = [];
    public IReadOnlyCollection<Account> ChildAccounts => _childAccounts;

    public bool IsRootLevel => HierarchyLevel == 0;

    public static Account Create(
        string name,
        string number,
        AccountType type,
        Currency currency,
        bool isPlaceholder,
        string description = "")
    {
        return new Account
        {   
            Name = name,
            Type = type,
            Number = number,
            Currency = currency,
            Description = description,
            IsPlaceholder = isPlaceholder,
        };
    }

    public Result AddChildAccount(Account account)
    {
        if (account.Equals(this))
            return Result.Invalid(AccountErrors.AddAccountToItself());
        
        if (!IsPlaceholder)
            return Result.Invalid(AccountErrors.NoChildrenWhenNotPlaceholder());

        if (account.Type != Type)
            return Result.Invalid(AccountErrors.ChildHasDifferentType(expected: Type, actual: account.Type));

        account.ParentAccountId = Id;
        account.HierarchyLevel = HierarchyLevel + 1;
        _childAccounts.Add(account);
        
        return Result.Success();
    }

    public Result RemoveChildAccount(Account account)
    {
        if (!IsPlaceholder)
            return Result.Invalid(AccountErrors.NoChildrenWhenNotPlaceholder());
        
        if (!_childAccounts.Remove(account))
            return Result.NotFound($"Account '{account}' is not in {this}.");

        account.ParentAccountId = null;
        account.HierarchyLevel = 0;
        return Result.Success();
    }

    public override string ToString() => $"{Number} - {Name}";
}