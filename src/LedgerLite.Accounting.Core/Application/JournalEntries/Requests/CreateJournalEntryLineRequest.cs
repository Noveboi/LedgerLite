namespace LedgerLite.Accounting.Core.Application.JournalEntries.Requests;

public sealed record CreateJournalEntryLineRequest(
    Guid AccountId,
    decimal Amount);