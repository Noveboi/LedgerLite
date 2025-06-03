using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.JournalEntries.Requests;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure;

// ReSharper disable once ConvertClosureToMethodGroup
namespace LedgerLite.Accounting.Core.Application.JournalEntries;

internal sealed class TransactionRecordingService(IAccountingUnitOfWork unitOfWork) : ITransactionRecordingService
{
    public async Task<Result<JournalEntry>> RecordStandardEntryAsync(RecordStandardEntryRequest req,
        CancellationToken ct)
    {
        if (await unitOfWork.FiscalPeriodRepository.GetByIdAsync(id: req.FiscalPeriodId, token: ct) is not { } period)
            return Result.Invalid(
                TransactionRecordingErrors.FiscalPeriodNotFound(periodId: req.FiscalPeriodId));

        return await CreateStandardJournalEntry(request: req, period: period)
            .Bind(entry => AddCreditLine(journalEntry: entry, request: req.CreditLine))
            .Bind(entry => AddDebitLine(journalEntry: entry, request: req.DebitLine))
            .Bind(entry => AddJournalEntryToRepository(entry: entry))
            .BindAsync(entry => SaveChangesAsync(entry: entry, token: ct));
    }

    private static Result<JournalEntry> CreateStandardJournalEntry(RecordStandardEntryRequest request,
        FiscalPeriod period)
    {
        return JournalEntry.Create(
            type: JournalEntryType.Standard,
            referenceNumber: request.ReferenceNumber,
            description: request.Description,
            occursAt: request.OccursAt,
            createdByUserId: request.RequestedByUserId,
            fiscalPeriod: period);
    }

    private static Result<JournalEntry> AddCreditLine(JournalEntry journalEntry, CreateJournalEntryLineRequest request)
    {
        return journalEntry.AddLine(
            accountId: request.AccountId,
            type: TransactionType.Credit,
            amount: request.Amount);
    }

    private static Result<JournalEntry> AddDebitLine(JournalEntry journalEntry, CreateJournalEntryLineRequest request)
    {
        return journalEntry.AddLine(
            accountId: request.AccountId,
            type: TransactionType.Debit,
            amount: request.Amount);
    }

    private Result<JournalEntry> AddJournalEntryToRepository(JournalEntry entry)
    {
        unitOfWork.JournalEntryRepository.Add(entry: entry);
        return Result.Success(value: entry);
    }

    private async Task<Result<JournalEntry>> SaveChangesAsync(JournalEntry entry, CancellationToken token)
    {
        await unitOfWork.SaveChangesAsync(token: token);
        return Result.Success(value: entry);
    }
}