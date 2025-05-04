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
            .RuleFor("_accounts", (f, c) => options?.Nodes?.ToList()
                                       ?? AccountFaker
                                           .GenerateLazy(f.Random.Number(1, 3))
                                           .Select(x => AccountNode.CreateRoot(c.Id, x))
                                           .ToList());
    } 
    
    public static ChartOfAccounts Empty => ChartOfAccounts.Create();
}