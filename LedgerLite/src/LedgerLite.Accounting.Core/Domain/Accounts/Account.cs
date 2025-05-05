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
    
    public static Result<Account> Create(
        string name,
        string number,
        AccountType type,
        Currency currency,
        bool isPlaceholder,
        string description = "")
    {
        if (string.IsNullOrWhiteSpace(number))
            return Result.Invalid(AccountErrors.AccountNumberIsEmpty());
        
        if (string.IsNullOrWhiteSpace(name))
            return Result.Invalid(AccountErrors.AccountNameIsEmpty());
        
        if (number.Length > 5)
            return Result.Invalid(AccountErrors.AccountNumberTooLong());
        
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

    public override string ToString() => $"{Number} - {Name}";
}