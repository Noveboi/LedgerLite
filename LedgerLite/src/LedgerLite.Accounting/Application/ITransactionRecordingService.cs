using Ardalis.Result;
using LedgerLite.Accounting.Domain.JournalEntries;

namespace LedgerLite.Accounting.Application;

/// <summary>
/// Orchestrates use cases involving the recording of financial transactions
/// </summary>
public interface ITransactionRecordingService
{
    Task<Result<JournalEntry>> RecordStandardEntryAsync(RecordStandardEntryRequest req, CancellationToken ct);
}