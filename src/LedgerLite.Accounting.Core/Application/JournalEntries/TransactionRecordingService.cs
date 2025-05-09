using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.JournalEntries.Requests;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure;
using Serilog;

// ReSharper disable once ConvertClosureToMethodGroup
namespace LedgerLite.Accounting.Core.Application.JournalEntries;

internal sealed class TransactionRecordingService(IAccountingUnitOfWork unitOfWork) : ITransactionRecordingService
{
    public async Task<Result<JournalEntry>> RecordStandardEntryAsync(RecordStandardEntryRequest req, CancellationToken ct)
    {
        if (await unitOfWork.FiscalPeriodRepository.GetByIdAsync(req.FiscalPeriodId, ct) is not { } period)
        {
            return Result.Invalid(TransactionRecordingErrors.FiscalPeriodNotFound(req.FiscalPeriodId));
        }
        
        return await CreateStandardJournalEntry(req, period)
            .Bind(entry => AddCreditLine(entry, req.CreditLine))
            .Bind(entry => AddDebitLine(entry, req.DebitLine))
            .Bind(entry => AddJournalEntryToRepository(entry))
            .BindAsync(entry => SaveChangesAsync(entry, ct));
    }

    private static Result<JournalEntry> CreateStandardJournalEntry(RecordStandardEntryRequest request, FiscalPeriod period) => 
        JournalEntry.Create(
            type: JournalEntryType.Standard,
            referenceNumber: request.ReferenceNumber,
            description: request.Description,
            occursAtUtc: request.OccursAtUtc,
            createdByUserId: request.RequestedByUserId,
            fiscalPeriod: period);
    
    private static Result<JournalEntry> AddCreditLine(JournalEntry journalEntry, CreateJournalEntryLineRequest request) =>
        journalEntry.AddLine(
            accountId: request.AccountId,
            type: TransactionType.Credit,
            amount: request.Amount);
    
    private static Result<JournalEntry> AddDebitLine(JournalEntry journalEntry, CreateJournalEntryLineRequest request) =>
        journalEntry.AddLine(
            accountId: request.AccountId,
            type: TransactionType.Debit,
            amount: request.Amount);

    private Result<JournalEntry> AddJournalEntryToRepository(JournalEntry entry)
    {
        unitOfWork.JournalEntryRepository.Add(entry);
        return Result.Success(entry);
    }

    private async Task<Result<JournalEntry>> SaveChangesAsync(JournalEntry entry, CancellationToken token)
    {
        await unitOfWork.SaveChangesAsync(token);
        return Result.Success(entry);
    }
}