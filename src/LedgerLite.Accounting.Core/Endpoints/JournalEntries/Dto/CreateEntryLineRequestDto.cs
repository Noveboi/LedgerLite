namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

internal sealed record CreateEntryLineRequestDto(
    Guid AccountId,
    decimal Amount);