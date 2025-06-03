namespace LedgerLite.Accounting.Core.Domain.JournalEntries;

public sealed record UpdateLineRequest(Guid LineId, TransactionType? Type, decimal? Amount, Guid? TransferAccountId);