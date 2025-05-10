using FastEndpoints;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries;

internal sealed class JournalEntryEndpointGroup : Group
{
    public JournalEntryEndpointGroup()
    {
        Configure("entries", _ => { });
    }
}