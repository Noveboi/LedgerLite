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
        return new JournalEntryResponseDto(entry.Id,
            entry.ReferenceNumber,
            entry.OccursAt,
            entry.Description,
            entry.Type.ToString(),
            entry.Status.ToString());
    }
}