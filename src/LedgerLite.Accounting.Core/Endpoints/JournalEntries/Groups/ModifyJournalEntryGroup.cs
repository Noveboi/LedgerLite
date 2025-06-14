﻿using FastEndpoints;
using LedgerLite.Accounting.Core.Authorization;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Groups;

internal sealed class ModifyJournalEntryGroup : SubGroup<JournalEntryEndpointGroup>
{
    public ModifyJournalEntryGroup()
    {
        Configure(routePrefix: "", ep => ep.Policy(x => x.RequireModificationPermissions()));
    }
}