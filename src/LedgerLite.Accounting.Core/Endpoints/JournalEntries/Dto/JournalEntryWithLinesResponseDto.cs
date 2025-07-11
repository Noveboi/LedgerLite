﻿using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

public sealed record JournalEntryWithLinesResponseDto(
    JournalEntryResponseDto Entry,
    IEnumerable<JournalEntryLineDto> Lines)
{
    public static JournalEntryWithLinesResponseDto FromEntity(JournalEntry entry)
    {
        return new JournalEntryWithLinesResponseDto(
            JournalEntryResponseDto.FromEntity(entry: entry),
            entry.Lines.Select(selector: JournalEntryLineDto.FromEntity));
    }
}