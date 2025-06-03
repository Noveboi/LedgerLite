using FastEndpoints;
using LedgerLite.Accounting.Core.Authorization;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Groups;

public class ChartOfAccountsModifyGroup : SubGroup<ChartOfAccountsEndpointGroup>
{
    public ChartOfAccountsModifyGroup()
    {
        Configure(routePrefix: "", ep => ep.Policy(x => x.RequireModificationPermissions()));
    }
}