using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Application.JournalEntries;

public interface IJournalEntryRepository
{
    void Add(JournalEntry entry);
    void Remove(JournalEntry entry);
}