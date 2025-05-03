namespace LedgerLite.Accounting.Application;

public sealed record RecordStandardEntryRequest(
    string ReferenceNumber,
    DateTime OccursAtUtc, 
    string Description,
    CreateJournalEntryLineRequest CreditLine,
    CreateJournalEntryLineRequest DebitLine);