using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Endpoints.Accounts.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.JournalEntries.Dto;

public sealed record JournalEntryLineDto(
    Guid Id,
    Guid EntryId,
    DateOnly OccursAt,
    string EntryDescription,
    double Credit,
    double Debit,
    SlimAccountDto Account,
    SlimAccountDto TransferAccount)
{
    public static JournalEntryLineDto FromEntity(JournalEntryLine line)
    {
        return new JournalEntryLineDto(
            line.Id,
            line.EntryId,
            line.Entry.OccursAt,
            line.Entry.Description,
            Account: SlimAccountDto.FromEntity(line.Account),
            TransferAccount: SlimAccountDto.FromEntity(line.GetTransferAccount()),
            Credit: line.TransactionType == TransactionType.Credit ? decimal.ToDouble(line.Amount) : 0,
            Debit: line.TransactionType == TransactionType.Debit ? decimal.ToDouble(line.Amount) : 0);
    }
}