using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;

namespace LedgerLite.Accounting.Core.Application.JournalEntries;

internal sealed class JournalEntryService(IJournalEntryRepository repository) : IJournalEntryService
{
    public async Task<Result<JournalEntry>> GetByIdAsync(Guid id, CancellationToken token) =>
        await repository.GetByIdAsync(id, token) ?? 
        Result<JournalEntry>.NotFound($"Couldn't find Journal Entry with ID '{id}'");
}