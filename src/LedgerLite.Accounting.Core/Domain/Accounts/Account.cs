using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Accounts.Metadata;
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
    public AccountMetadata Metadata { get; private init; } = null!;

    public static Result<Account> Create(
        string name,
        string number,
        AccountType type,
        Currency currency,
        bool isPlaceholder,
        string description = "",
        AccountMetadata? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(value: number))
            return Result.Invalid(AccountErrors.AccountNumberIsEmpty());

        if (string.IsNullOrWhiteSpace(value: name))
            return Result.Invalid(AccountErrors.AccountNameIsEmpty());

        if (number.Length > 5)
            return Result.Invalid(AccountErrors.AccountNumberTooLong());

        if (metadata?.Verify(accountType: type) is { IsSuccess: false } result)
            return result.Map();

        return new Account
        {
            Name = name,
            Type = type,
            Number = number,
            Currency = currency,
            Description = description,
            IsPlaceholder = isPlaceholder,
            Metadata = metadata ?? AccountMetadata.Default
        };
    }

    public override string ToString()
    {
        return $"{Number} - {Name}";
    }
}