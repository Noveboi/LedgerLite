using LedgerLite.Accounting.Core.Domain.JournalEntries;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed class JournalEntryRepository(AccountingDbContext context) : IJournalEntryRepository
{
    public void Add(JournalEntry entry) => context.JournalEntries.Add(entry);
    public void Remove(JournalEntry entry) => context.JournalEntries.Remove(entry);

    public Task<JournalEntry?> GetByIdAsync(Guid id, CancellationToken token) =>
        context.JournalEntries.FirstOrDefaultAsync(x => x.Id == id, token);

    public async Task<IReadOnlyList<JournalEntry>> GetByFiscalPeriodIdAsync(Guid fiscalPeriodId, CancellationToken token) =>
        await context.JournalEntries
            .Include(x => x.Lines)
            .ThenInclude(x => x.Account)
            .Where(x => x.FiscalPeriodId == fiscalPeriodId)
            .ToListAsync(token);
}