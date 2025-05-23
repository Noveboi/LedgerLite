using FastEndpoints;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods;

public sealed class FiscalPeriodEndpointGroup : Group
{
    public FiscalPeriodEndpointGroup()
    {
        Configure("periods", _ => { });
    }
}