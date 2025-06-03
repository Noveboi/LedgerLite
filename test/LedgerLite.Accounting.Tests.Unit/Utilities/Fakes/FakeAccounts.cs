using Bogus;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Accounts.Metadata;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

public sealed class FakeAccountOptions
{
    public string? Name { get; set; }
    public bool? IsPlaceholder { get; set; }
    public AccountType? Type { get; set; }
    public Currency? Currency { get; set; }
    public ExpenseType ExpenseType { get; set; } = ExpenseType.Undefined;

    public FakeAccountOptions WithName(string name, AccountType type)
    {
        Name = name;
        Type = type;
        return this;
    }
    
    public FakeAccountOptions WithType(AccountType type)
    {
        Type = type;
        return this;
    }

    public FakeAccountOptions WithExpenseType(ExpenseType expenseType)
    {
        ExpenseType = expenseType;
        return this;
    }
}

public static class FakeAccounts
{
    public static Faker<Account> GetAccountFaker(FakeAccountOptions? options = null)
    {
        return new PrivateFaker<Account>()
            .UsePrivateConstructor()
            .RuleFor(x => x.Currency, options?.Currency ?? Currency.Euro)
            .RuleFor(x => x.Name, f => options?.Name ?? f.Lorem.Word())
            .RuleFor(x => x.Number, f => f.Random.String2(length: 3, chars: "0123456789"))
            .RuleFor(x => x.Type, f => options?.Type ?? f.PickRandom((IEnumerable<AccountType>)AccountType.List))
            .RuleFor(x => x.IsPlaceholder, options?.IsPlaceholder ?? false)
            .RuleFor(x => x.Metadata, _ => new AccountMetadata(
                ExpenseType: options?.ExpenseType ?? ExpenseType.Undefined));
    }

    public static Account NewAccount()
    {
        return GetAccountFaker().Generate();
    }

    public static Account Get(Action<FakeAccountOptions>? configure = null)
    {
        var options = new FakeAccountOptions();
        configure?.Invoke(obj: options);
        return GetAccountFaker(options: options).Generate();
    }

    public static Account GetPlaceholder(Action<FakeAccountOptions>? configure = null)
    {
        var options = new FakeAccountOptions();
        configure?.Invoke(obj: options);
        options.IsPlaceholder = true;
        return GetAccountFaker(options: options).Generate();
    }
}