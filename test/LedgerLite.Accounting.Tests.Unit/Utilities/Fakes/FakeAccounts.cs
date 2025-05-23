using Bogus;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

public sealed class FakeAccountOptions
{
    public string? Name { get; set; }
    public bool? IsPlaceholder { get; set; }
    public AccountType? Type { get; set; }
    public Currency? Currency { get; set; }

    public FakeAccountOptions WithName(string name, AccountType type)
    {
        Name = name;
        Type = type;
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
            .RuleFor(x => x.Name, f => f.Lorem.Word())
            .RuleFor(x => x.Number, f => f.Random.String2(3, "0123456789"))
            .RuleFor(x => x.Type, f => options?.Type ?? f.PickRandom((IEnumerable<AccountType>)AccountType.List))
            .RuleFor(x => x.IsPlaceholder, options?.IsPlaceholder ?? false);
    }

    public static Account NewAccount() => GetAccountFaker().Generate();
    public static List<Account> NewAccounts(int count) => GetAccountFaker().Generate(count);

    public static Account Get(Action<FakeAccountOptions>? configure = null)
    {
        var options = new FakeAccountOptions();
        configure?.Invoke(options);
        return GetAccountFaker(options).Generate();
    }

    public static Account GetPlaceholder(Action<FakeAccountOptions>? configure = null)
    {
        var options = new FakeAccountOptions();
        configure?.Invoke(options);
        options.IsPlaceholder = true;
        return GetAccountFaker(options).Generate();
    }
}