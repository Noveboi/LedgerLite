using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

public sealed record JournalEntryWithLinesResponseDto(
    JournalEntryResponseDto Entry,
    IEnumerable<JournalEntryLineResponseDto> Lines)
{
    public static JournalEntryWithLinesResponseDto FromEntity(JournalEntry entry) => new(
        Entry: JournalEntryResponseDto.FromEntity(entry),
        Lines: entry.Lines.Select(JournalEntryLineResponseDto.FromEntity));
}

public sealed record JournalEntryLineResponseDto(
    Guid Id,
    Guid EntryId,
    SlimAccountDto Account,
    double Credit,
    double Debit)
{
    public static JournalEntryLineResponseDto FromEntity(JournalEntryLine line) => new(
        Id: line.Id,
        EntryId: line.EntryId,
        Account: SlimAccountDto.FromEntity(line.Account),
        Credit: line.TransactionType == TransactionType.Credit ? decimal.ToDouble(line.Amount) : 0,
        Debit: line.TransactionType == TransactionType.Debit ? decimal.ToDouble(line.Amount) : 0);
}