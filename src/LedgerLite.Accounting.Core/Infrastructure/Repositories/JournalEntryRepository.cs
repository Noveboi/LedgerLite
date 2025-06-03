using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.SharedKernel.Models;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed class JournalEntryRepository(AccountingDbContext context) : IJournalEntryRepository
{
    public void Add(JournalEntry entry)
    {
        context.JournalEntries.Add(entity: entry);
    }

    public void Remove(JournalEntry entry)
    {
        context.JournalEntries.Remove(entity: entry);
    }

    public Task<JournalEntry?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return context.JournalEntries.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
    }
}