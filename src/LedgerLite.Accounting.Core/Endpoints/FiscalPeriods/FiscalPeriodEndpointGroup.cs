using FastEndpoints;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods;

internal sealed class FiscalPeriodEndpointGroup : Group
{
    public FiscalPeriodEndpointGroup()
    {
        Configure("periods", _ => { });
    }
}