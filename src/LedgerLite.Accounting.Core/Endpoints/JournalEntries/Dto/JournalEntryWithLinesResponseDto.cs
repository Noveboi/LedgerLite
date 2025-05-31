using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

public sealed record JournalEntryWithLinesResponseDto(
    JournalEntryResponseDto Entry,
    IEnumerable<JournalEntryLineDto> Lines)
{
    public static JournalEntryWithLinesResponseDto FromEntity(JournalEntry entry)
    {
        return new JournalEntryWithLinesResponseDto(
            Entry: JournalEntryResponseDto.FromEntity(entry: entry),
            Lines: entry.Lines.Select(selector: JournalEntryLineDto.FromEntity));
    }
}