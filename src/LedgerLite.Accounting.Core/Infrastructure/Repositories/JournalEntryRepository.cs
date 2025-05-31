using LedgerLite.Accounting.Core.Domain.JournalEntries;
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
        return context.JournalEntries.FirstOrDefaultAsync(predicate: x => x.Id == id, cancellationToken: token);
    }

    public async Task<IReadOnlyList<JournalEntry>> GetByFiscalPeriodIdAsync(Guid fiscalPeriodId,
        CancellationToken token)
    {
        return await context.JournalEntries
            .Include(navigationPropertyPath: x => x.Lines)
            .ThenInclude(navigationPropertyPath: x => x.Account)
            .Where(predicate: x => x.FiscalPeriodId == fiscalPeriodId)
            .ToListAsync(cancellationToken: token);
    }
}