using FastEndpoints;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts;

internal sealed class AccountEndpointsGroup : Group
{
    public AccountEndpointsGroup()
    {
        Configure("accounts", endpoint =>
        {
            // endpoint.Roles("Account.Read");
        });
    }
}