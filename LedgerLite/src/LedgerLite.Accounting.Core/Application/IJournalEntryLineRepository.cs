using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Application;

public interface IJournalEntryLineRepository
{
    void Add(JournalEntryLine line);
    void Remove(JournalEntryLine line);

    Task<IReadOnlyList<JournalEntryLine>> GetLinesForAccountAsync(Account account);
    Task<IReadOnlyList<JournalEntryLine>> GetLinesForEntryAsync(JournalEntry entry);
}