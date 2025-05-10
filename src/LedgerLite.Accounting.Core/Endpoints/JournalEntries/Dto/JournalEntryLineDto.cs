using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

public sealed record JournalEntryLineDto(
    Guid Id,
    Guid EntryId,
    string EntryDescription,
    double Credit,
    double Debit,
    SlimAccountDto Account)
{
    public static JournalEntryLineDto FromEntity(JournalEntryLine line) => new(
        Id: line.Id,
        EntryId: line.EntryId,
        EntryDescription: line.Entry.Description,
        Account: SlimAccountDto.FromEntity(line.Account),
        Credit: line.TransactionType == TransactionType.Credit ? decimal.ToDouble(line.Amount) : 0,
        Debit: line.TransactionType == TransactionType.Debit ? decimal.ToDouble(line.Amount) : 0);
}