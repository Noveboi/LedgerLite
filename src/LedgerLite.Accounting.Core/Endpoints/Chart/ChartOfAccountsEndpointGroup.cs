using FastEndpoints;

namespace LedgerLite.Accounting.Core.Endpoints.Chart;

public sealed class ChartOfAccountsEndpointGroup : Group
{
    public ChartOfAccountsEndpointGroup()
    {
        Configure("chart-of-accounts", _ => { });
    }
}