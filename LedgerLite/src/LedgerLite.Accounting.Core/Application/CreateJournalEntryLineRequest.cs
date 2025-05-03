namespace LedgerLite.Accounting.Core.Application;

public sealed record CreateJournalEntryLineRequest(
    Guid AccountId,
    decimal Amount);