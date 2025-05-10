using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Domain.Chart;

/// <summary>
/// Holds information for an account, its parent/child relationships and all its journal entry lines. 
/// </summary>
public sealed record AccountWithDetails(
    AccountNode Node,
    IEnumerable<JournalEntryLine> Lines);