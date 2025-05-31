using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed record JournalEntryLineQueryOptions(Guid? FiscalPeriodId);

internal sealed class JournalEntryLineRepository(AccountingDbContext context) : IJournalEntryLineRepository
{
    public void Add(JournalEntryLine line)
    {
        context.JournalEntryLines.Add(entity: line);
    }

    public void Remove(JournalEntryLine line)
    {
        context.JournalEntryLines.Remove(entity: line);
    }

    public async Task<IReadOnlyList<JournalEntryLine>> GetLinesForAccountAsync(
        Account account,
        JournalEntryLineQueryOptions? options,
        CancellationToken ct)
    {
        var query = context.JournalEntryLines
            .Include(navigationPropertyPath: x => x.Entry)
            .ThenInclude(navigationPropertyPath: x => x.Lines)
            .Where(predicate: x => x.AccountId == account.Id);


        if (options is null)
            return await query.ToListAsync(cancellationToken: ct);

        if (options.FiscalPeriodId is not null)
            query = query.Where(predicate: x => x.Entry.FiscalPeriodId == options.FiscalPeriodId.Value);

        return await query.ToListAsync(cancellationToken: ct);
    }

    public async Task<IReadOnlyList<JournalEntryLine>> GetLinesForEntryAsync(JournalEntry entry, CancellationToken ct)
    {
        return await context.JournalEntryLines
            .Where(predicate: x => x.EntryId == entry.Id)
            .ToListAsync(cancellationToken: ct);
    }
}