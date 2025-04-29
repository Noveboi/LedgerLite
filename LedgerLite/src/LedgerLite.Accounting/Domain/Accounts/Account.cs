using Ardalis.Result;
using LedgerLite.SharedKernel;

namespace LedgerLite.Accounting.Domain.Accounts;

public sealed class Account : AuditableEntity
{
    private Account() { }
    
    public string Number { get; private init; } = null!;
    public string Name { get; private init; } = null!;
    public string Description { get; private init; } = "";

    public AccountType Type { get; private init; } = null!;
    public Currency Currency { get; private init; } = null!;
    public bool IsPlaceholder { get; private init; }
    
    public Guid? ParentAccountId { get; private set; }

    private readonly List<Account> _childAccounts = [];
    public IReadOnlyCollection<Account> ChildAccounts => _childAccounts;

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
            Number = number,
            Type = type,
            Currency = currency,
            Description = description,
            IsPlaceholder = isPlaceholder,
        };
    }

    public Result AddChildAccount(Account account)
    {
        if (!IsPlaceholder)
            return Result.Invalid();

        if (account.Type != Type)
            return Result.Invalid();

        account.ParentAccountId = Id;
        _childAccounts.Add(account);
        
        return Result.Success();
    }
}