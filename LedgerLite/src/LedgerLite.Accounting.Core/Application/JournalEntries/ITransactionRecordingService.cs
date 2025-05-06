using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.JournalEntries.Requests;
using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Core.Application.JournalEntries;

/// <summary>
/// Orchestrates use cases involving the recording of financial transactions
/// </summary>
public interface ITransactionRecordingService
{
    Task<Result<JournalEntry>> RecordStandardEntryAsync(RecordStandardEntryRequest req, CancellationToken ct);
}