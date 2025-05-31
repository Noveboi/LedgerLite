using LedgerLite.Accounting.Core.Domain.Chart;

namespace LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;

internal static class FakeAccountNodes
{
    private static readonly Guid ChartId = Guid.NewGuid();

    public static readonly AccountNode SampleChild =
        AccountNode.Create(chartId: ChartId, account: FakeAccounts.NewAccount());

    public static AccountNode Get(Action<FakeAccountOptions>? configure = null)
    {
        return AccountNode.Create(
            chartId: Guid.NewGuid(),
            account: FakeAccounts.Get(configure: configure ?? (o =>
            {
                o.IsPlaceholder = true;
                o.Type = SampleChild.Account.Type;
            })));
    }
}