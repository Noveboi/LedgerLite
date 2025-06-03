using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

internal sealed record AccountWithLinesDto
{
    public required AccountDto Account { get; init; }
    public required IEnumerable<JournalEntryLineDto> Lines { get; init; }
}