using LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

internal sealed record AccountWithLinesDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Number { get; init; }
    public required string Type { get; init; }
    public required string Currency { get; init; }
    public required bool IsControl { get; init; }

    public required IEnumerable<JournalEntryLineDto> Lines { get; init; }
}