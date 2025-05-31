using FastEndpoints;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Groups;

namespace LedgerLite.Accounting.Reporting.Trial.Endpoints;

internal sealed class TrialBalanceEndpointGroup : SubGroup<FiscalPeriodEndpointGroup>
{
    public TrialBalanceEndpointGroup()
    {
        Configure("{periodId:guid}/trial-balance", _ => { });
    }
}