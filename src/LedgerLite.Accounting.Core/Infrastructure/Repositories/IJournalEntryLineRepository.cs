using LedgerLite.Accounting.Core.Domain.Accounts;
using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Infrastructure.Repositories;

internal interface IJournalEntryLineRepository
{
    void Add(JournalEntryLine line);
    void Remove(JournalEntryLine line);

    Task<IReadOnlyList<JournalEntryLine>> GetLinesForAccountAsync(
        Account account,
        JournalEntryLineQueryOptions? options = null,
        CancellationToken ct = default);

    Task<JournalEntryLine?> GetByIdAsync(Guid id, Guid fiscalPeriodId, CancellationToken ct);
}