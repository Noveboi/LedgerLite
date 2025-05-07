namespace LedgerLite.Accounting.Core.Application.JournalEntries.Requests;

public sealed record RecordStandardEntryRequest(
    string ReferenceNumber,
    DateTime OccursAtUtc, 
    string Description,
    CreateJournalEntryLineRequest CreditLine,
    CreateJournalEntryLineRequest DebitLine);