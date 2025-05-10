using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Application.JournalEntries;

/// <summary>
/// Handles simple operations regarding journal entries.
/// For more actions regarding journal entries, see <see cref="ITransactionRecordingService"/>
/// </summary>
public interface IJournalEntryService
{
    Task<Result<JournalEntry>> GetByIdAsync(Guid id, CancellationToken token);
}