using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

public sealed record JournalEntryWithLinesResponseDto(
    JournalEntryResponseDto Entry,
    IEnumerable<JournalEntryLineDto> Lines)
{
    public static JournalEntryWithLinesResponseDto FromEntity(JournalEntry entry) => new(
        Entry: JournalEntryResponseDto.FromEntity(entry),
        Lines: entry.Lines.Select(JournalEntryLineDto.FromEntity));
}