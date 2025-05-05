using LedgerLite.Accounting.Core.Application;
using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Infrastructure;

internal sealed class JournalEntryRepository(AccountingDbContext context) : IJournalEntryRepository
{
    public void Add(JournalEntry entry) => context.JournalEntries.Add(entry);
    public void Remove(JournalEntry entry) => context.JournalEntries.Remove(entry);
}