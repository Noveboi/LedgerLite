using FastEndpoints;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Groups;

public sealed class ChartOfAccountsEndpointGroup : Group
{
    public ChartOfAccountsEndpointGroup()
    {
        Configure("accounts", _ => { });
    }
}