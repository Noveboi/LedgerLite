using FastEndpoints;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Groups;

namespace LedgerLite.Accounting.Reporting.Income.Endpoints;

internal sealed class IncomeEndpointGroup : SubGroup<FiscalPeriodEndpointGroup>
{
    public IncomeEndpointGroup()
    {
        Configure("{periodId:guid}/income", _ => { });
    }
}