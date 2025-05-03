using LedgerLite.Accounting.Domain.JournalEntries;

namespace LedgerLite.Accounting.Infrastructure.Persistence;

public interface IJournalEntryRepository
{
    void Add(JournalEntry entry);
    void Remove(JournalEntry entry);
}