using FastEndpoints;
using LedgerLite.Accounting.Core.Authorization;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Groups;

internal sealed class ModifyFiscalPeriodGroup : SubGroup<FiscalPeriodEndpointGroup>
{
    public ModifyFiscalPeriodGroup()
    {
        Configure(routePrefix: "", ep: ep => ep.Policy(policy: x => x.RequireModificationPermissions()));
    }
}