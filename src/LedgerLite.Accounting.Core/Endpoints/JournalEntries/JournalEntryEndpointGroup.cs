using FastEndpoints;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries;

internal sealed class JournalEntryEndpointGroup : SubGroup<FiscalPeriodEndpointGroup>
{
    public JournalEntryEndpointGroup()
    {
        Configure("{fiscalPeriodId:guid}/entries", _ => { });
    }
}