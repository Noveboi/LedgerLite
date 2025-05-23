﻿using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal sealed class JournalEntryLineRepository(AccountingDbContext context) : IJournalEntryLineRepository
{
    public void Add(JournalEntryLine line) => context.JournalEntryLines.Add(line);

    public void Remove(JournalEntryLine line) => context.JournalEntryLines.Remove(line);

    public async Task<IReadOnlyList<JournalEntryLine>> GetLinesForAccountAsync(Account account) =>
        await context.JournalEntryLines
            .Include(x => x.Entry)
            .Where(x => x.AccountId == account.Id)
            .ToListAsync();

    public async Task<IReadOnlyList<JournalEntryLine>> GetLinesForEntryAsync(JournalEntry entry) =>
        await context.JournalEntryLines
            .Where(x => x.EntryId == entry.Id)
            .ToListAsync();
}