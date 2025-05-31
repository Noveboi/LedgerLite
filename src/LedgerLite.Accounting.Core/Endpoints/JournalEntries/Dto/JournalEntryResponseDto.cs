using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

public sealed record JournalEntryResponseDto(
    Guid Id,
    string ReferenceNumber,
    DateOnly OccursAtUtc,
    string Description,
    string Type,
    string Status)
{
    public static JournalEntryResponseDto FromEntity(JournalEntry entry)
    {
        return new JournalEntryResponseDto(Id: entry.Id,
            ReferenceNumber: entry.ReferenceNumber,
            OccursAtUtc: entry.OccursAt,
            Description: entry.Description,
            Type: entry.Type.ToString(),
            Status: entry.Status.ToString());
    }
}