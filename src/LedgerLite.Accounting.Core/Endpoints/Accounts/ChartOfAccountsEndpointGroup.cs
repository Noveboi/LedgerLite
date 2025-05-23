using FastEndpoints;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

public sealed class ChartOfAccountsEndpointGroup : Group
{
    public ChartOfAccountsEndpointGroup()
    {
        Configure("accounts", _ => { });
    }
}