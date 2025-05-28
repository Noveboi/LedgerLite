using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed record JournalEntryLineQueryOptions(Guid? FiscalPeriodId);

internal sealed class JournalEntryLineRepository(AccountingDbContext context) : IJournalEntryLineRepository
{
    public void Add(JournalEntryLine line) => context.JournalEntryLines.Add(line);

    public void Remove(JournalEntryLine line) => context.JournalEntryLines.Remove(line);

    public async Task<IReadOnlyList<JournalEntryLine>> GetLinesForAccountAsync(
        Account account, 
        JournalEntryLineQueryOptions? options,
        CancellationToken ct)
    {
        var query = context.JournalEntryLines
            .Include(x => x.Entry)
            .ThenInclude(x => x.Lines)
            .Where(x => x.AccountId == account.Id);
            

        if (options is null)
            return await query.ToListAsync(ct);

        if (options.FiscalPeriodId is not null)
        {
            query = query.Where(x => x.Entry.FiscalPeriodId == options.FiscalPeriodId.Value);
        }

        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<JournalEntryLine>> GetLinesForEntryAsync(JournalEntry entry, CancellationToken ct) =>
        await context.JournalEntryLines
            .Where(x => x.EntryId == entry.Id)
            .ToListAsync(ct);
}