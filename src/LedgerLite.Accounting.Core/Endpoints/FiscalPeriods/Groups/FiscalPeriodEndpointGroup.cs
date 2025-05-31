using FastEndpoints;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Groups;

public sealed class FiscalPeriodEndpointGroup : Group
{
    public FiscalPeriodEndpointGroup()
    {
        Configure("periods", _ => { });
    }
}