using FastEndpoints;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Groups;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Groups;

internal sealed class JournalEntryEndpointGroup : SubGroup<FiscalPeriodEndpointGroup>
{
    public JournalEntryEndpointGroup()
    {
        Configure("{fiscalPeriodId:guid}/entries", _ => { });
    }
}