using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

public sealed record JournalEntryResponseDto(
    Guid Id,
    string ReferenceNumber,
    DateTime OccursAtUtc,
    string Description,
    string Type,
    string Status)
{
    public static JournalEntryResponseDto FromEntity(JournalEntry entry) =>
        new(Id: entry.Id,
            ReferenceNumber: entry.ReferenceNumber,
            OccursAtUtc: entry.OccursAtUtc,
            Description: entry.Description,
            Type: entry.Type.ToString(),
            Status: entry.Status.ToString());
}