using FastEndpoints;
using LedgerLite.Accounting.Core.Endpoints.Chart;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed class AccountEndpointsGroup : SubGroup<ChartOfAccountsEndpointGroup>
{
    public AccountEndpointsGroup()
    {
        Configure("/accounts", endpoint =>
        {
            // endpoint.Roles("Account.Read");
        });
    }
}