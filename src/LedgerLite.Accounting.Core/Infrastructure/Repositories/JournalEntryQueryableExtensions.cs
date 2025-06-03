using LedgerLite.Accounting.Core.Domain.JournalEntries;
using Microsoft.EntityFrameworkCore;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

public static class JournalEntryQueryableExtensions
{
    public static IQueryable<JournalEntry> IncludeAccounts(this IQueryable<JournalEntry> set) => 
        set.Include(x => x.Lines)
            .ThenInclude(x => x.Account);
}