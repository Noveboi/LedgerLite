using Bogus;
using LedgerLite.Accounting.Domain;
using LedgerLite.Accounting.Domain.Accounts;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

public sealed class FakeAccountOptions
{
    public Guid? ParentId { get; set; }
    public AccountType? Type { get; set; }
    public Currency? Currency { get; set; }
    public IEnumerable<Account>? Children { get; set; }
    public int HierarchyLevel { get; set; } = 0;
}
public static class FakeAccounts
{
    public static Faker<Account> GetAccountFaker(FakeAccountOptions? options = null)
    {
        return new PrivateFaker<Account>(new PrivateBinder())
            .UsePrivateConstructor()
            .RuleFor(x => x.Currency, options?.Currency ?? Currency.Euro)
            .RuleFor(x => x.Name, f => f.Lorem.Word())
            .RuleFor(x => x.Number, f => f.Random.String2(3, "0123456789"))
            .RuleFor(x => x.Type, f => options?.Type ?? f.PickRandom((IEnumerable<AccountType>)AccountType.List))
            .RuleFor(x => x.ParentAccountId, _ => options?.ParentId ?? null)
            .RuleFor(x => x.IsPlaceholder, options?.Children is not null)
            .RuleFor(x => x.HierarchyLevel, options?.HierarchyLevel ?? 0)
            .RuleFor("_childAccounts", _ => options?.Children?.ToList() ?? []);
    }

    public static Account NewAccount() => GetAccountFaker().Generate();
    public static List<Account> NewAccounts(int count) => GetAccountFaker().Generate(count);

    public static Account Get(Action<FakeAccountOptions>? configure = null)
    {
        var options = new FakeAccountOptions();
        configure?.Invoke(options);
        return GetAccountFaker(options).Generate();
    }
}