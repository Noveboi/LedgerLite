using LedgerLite.Accounting.Domain.JournalEntries;

namespace LedgerLite.Accounting.Infrastructure.Persistence;

public interface IJournalEntryLineRepository
{
    void Add(JournalEntryLine line);
    void Remove(JournalEntryLine line);

    Task<IReadOnlyList<JournalEntryLine>> GetLinesForAccountAsync(Guid accountId);
    Task<IReadOnlyList<JournalEntryLine>> GetLinesForEntryAsync(Guid entryId);
}