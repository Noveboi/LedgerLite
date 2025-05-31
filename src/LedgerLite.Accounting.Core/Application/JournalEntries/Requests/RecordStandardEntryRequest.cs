namespace LedgerLite.Accounting.Core.Application.JournalEntries.Requests;

public sealed record RecordStandardEntryRequest(
    string ReferenceNumber,
    DateOnly OccursAt,
    string Description,
    CreateJournalEntryLineRequest CreditLine,
    CreateJournalEntryLineRequest DebitLine,
    Guid RequestedByUserId,
    Guid FiscalPeriodId);