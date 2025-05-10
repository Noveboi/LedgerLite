using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Application.JournalEntries;

internal sealed class JournalEntryService : IJournalEntryService
{
    public Task<Result<JournalEntry>> GetByIdAsync(Guid id, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}