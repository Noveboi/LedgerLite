using Bogus;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities;

public sealed class ChartOfAccountsFakerOptions
{
    public Guid? Id { get; set; } 
    public IEnumerable<AccountNode>? Nodes { get; set; }
}

public static class FakeChartOfAccounts
{
    private static readonly Faker<Account> AccountFaker = FakeAccounts.GetAccountFaker(); 
    
    public static Faker<ChartOfAccounts> GetFaker(ChartOfAccountsFakerOptions? options = null)
    {
        return new PrivateFaker<ChartOfAccounts>(new PrivateBinder())
            .UsePrivateConstructor()
            .RuleFor(x => x.Id, (_, c) => options?.Id ?? c.Id)
            .RuleFor("_nodes", (f, c) => options?.Nodes?.ToList()
                                       ?? AccountFaker
                                           .GenerateLazy(f.Random.Number(1, 3))
                                           .Select(x => AccountNode.Create(c.Id, x))
                                           .ToList());
    }

    public static ChartOfAccounts With(params Account[] accounts)
    {
        var id = Guid.NewGuid();
        var faker = GetFaker(new ChartOfAccountsFakerOptions
        {
            Id = id,
            Nodes = accounts.Select(x => AccountNode.Create(id, x))
        });

        return faker.Generate();
    }
    public static ChartOfAccounts Empty => ChartOfAccounts.Create();
}