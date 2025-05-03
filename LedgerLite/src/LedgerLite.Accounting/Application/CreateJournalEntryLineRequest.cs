using LedgerLite.Accounting.Domain;

namespace LedgerLite.Accounting.Application;

public sealed record CreateJournalEntryLineRequest(
    Guid AccountId,
    decimal Amount);