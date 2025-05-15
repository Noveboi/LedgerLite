using Bogus;
using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.Chart;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

public sealed class ChartOfAccountsFakerOptions
{
    public Guid? Id { get; set; } 
    public Guid? OrganizationId { get; set; }
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
            .RuleFor(x => x.OrganizationId, _ => options?.OrganizationId ?? Guid.NewGuid())
            .RuleFor("_nodes", (f, c) => options?.Nodes?.ToList()
                                       ?? AccountFaker
                                           .GenerateLazy(f.Random.Number(1, 3))
                                           .Select(x => AccountNode.Create(c.Id, x))
                                           .ToList());
    }

    public static ChartOfAccounts With(params FakeNodeBuilder[] accounts)
    {
        var id = Guid.NewGuid();
        var faker = GetFaker(new ChartOfAccountsFakerOptions
        {
            Id = id,
            Nodes = accounts.SelectMany(x =>
            {
                var node = AccountNode.Create(id, x.Account);
                if (x.ConfigureChildren is null) 
                    return new List<AccountNode> { node };
                
                var builder = new FakeChartOfAccountsBuilder(node);
                x.ConfigureChildren(builder);

                return [node, ..builder.Children];
            })
        });

        return faker.Generate();
    }

    public static ChartOfAccounts Empty => GetFaker(new ChartOfAccountsFakerOptions
    {
        Nodes = []
    }).Generate();
}

public sealed record FakeNodeBuilder(Account Account, Action<FakeChartOfAccountsBuilder>? ConfigureChildren)
{
    public static implicit operator FakeNodeBuilder(Account account) => new(Account: account, ConfigureChildren: null);

    public static implicit operator FakeNodeBuilder(ValueTuple<Account, Action<FakeChartOfAccountsBuilder>> tuple) => new(
        Account: tuple.Item1,
        ConfigureChildren: tuple.Item2);
}

public sealed class FakeChartOfAccountsBuilder(AccountNode parent)
{
    internal List<AccountNode> Children { get; } = [];
    public FakeChartOfAccountsBuilder AddChild(Account child)
    {
        var childNode = AccountNode.Create(parent.ChartId, child);
        parent.AddChild(childNode);
        
        Children.Add(childNode);
        
        return this;
    }
}