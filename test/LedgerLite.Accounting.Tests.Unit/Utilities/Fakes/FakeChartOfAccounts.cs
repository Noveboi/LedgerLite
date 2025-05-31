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

    public static ChartOfAccounts Empty => GetFaker(options: new ChartOfAccountsFakerOptions
    {
        Nodes = []
    }).Generate();

    public static Faker<ChartOfAccounts> GetFaker(ChartOfAccountsFakerOptions? options = null)
    {
        return new PrivateFaker<ChartOfAccounts>(binder: new PrivateBinder())
            .UsePrivateConstructor()
            .RuleFor(property: x => x.Id, setter: (_, c) => options?.Id ?? c.Id)
            .RuleFor(property: x => x.OrganizationId, setter: _ => options?.OrganizationId ?? Guid.NewGuid())
            .RuleFor(propertyOrFieldName: "_nodes", setter: (f, c) => options?.Nodes?.ToList()
                                                                      ?? AccountFaker
                                                                          .GenerateLazy(
                                                                              count: f.Random.Number(min: 1, max: 3))
                                                                          .Select(selector: x =>
                                                                              AccountNode.Create(chartId: c.Id,
                                                                                  account: x))
                                                                          .ToList());
    }

    public static ChartOfAccounts With(params FakeNodeBuilder[] accounts)
    {
        var id = Guid.NewGuid();
        var faker = GetFaker(options: new ChartOfAccountsFakerOptions
        {
            Id = id,
            Nodes = accounts.SelectMany(selector: x =>
            {
                var node = AccountNode.Create(chartId: id, account: x.Account);
                if (x.ConfigureChildren is null)
                    return new List<AccountNode> { node };

                var builder = new FakeChartOfAccountsBuilder(parent: node);
                x.ConfigureChildren(obj: builder);

                return [node, ..builder.Children];
            })
        });

        return faker.Generate();
    }
}

public sealed record FakeNodeBuilder(Account Account, Action<FakeChartOfAccountsBuilder>? ConfigureChildren)
{
    public static implicit operator FakeNodeBuilder(Account account)
    {
        return new FakeNodeBuilder(Account: account, ConfigureChildren: null);
    }

    public static implicit operator FakeNodeBuilder(ValueTuple<Account, Action<FakeChartOfAccountsBuilder>> tuple)
    {
        return new FakeNodeBuilder(
            Account: tuple.Item1,
            ConfigureChildren: tuple.Item2);
    }
}

public sealed class FakeChartOfAccountsBuilder(AccountNode parent)
{
    internal List<AccountNode> Children { get; } = [];

    public FakeChartOfAccountsBuilder AddChild(Account child)
    {
        var childNode = AccountNode.Create(chartId: parent.ChartId, account: child);
        parent.AddChild(child: childNode);

        Children.Add(item: childNode);

        return this;
    }
}