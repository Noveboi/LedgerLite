using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

// ReSharper disable once ConvertClosureToMethodGroup
namespace LedgerLite.Accounting.Core.Application;

internal sealed class TransactionRecordingService(IAccountingUnitOfWork unitOfWork) : ITransactionRecordingService
{
    private static readonly ILogger Logger = Log.ForContext<TransactionRecordingService>();
    
    public async Task<Result<JournalEntry>> RecordStandardEntryAsync(RecordStandardEntryRequest req, CancellationToken ct) =>
        await CreateStandardJournalEntry(req)
            .Bind(entry => AddCreditLine(entry, req.CreditLine))
            .Bind(entry => AddDebitLine(entry, req.DebitLine))
            .Bind(entry => MarkJournalEntryAsAdded(entry))
            .BindAsync(entry => SaveChangesAsync(entry, ct));

    private static Result<JournalEntry> CreateStandardJournalEntry(RecordStandardEntryRequest request) => 
        JournalEntry.Create(
            type: JournalEntryType.Standard,
            referenceNumber: request.ReferenceNumber,
            description: request.Description,
            occursAtUtc: request.OccursAtUtc);
    
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

    private Result<JournalEntry> MarkJournalEntryAsAdded(JournalEntry entry)
    {
        unitOfWork.JournalEntryRepository.Add(entry);
        return Result.Success(entry);
    }

    private async Task<Result<JournalEntry>> SaveChangesAsync(JournalEntry entry, CancellationToken token)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(token);
            return Result.Success(entry);
        }
        catch (DbUpdateException ex)
        {
            Logger.Error(ex, "Couldn't persist changes to database.");
            return Result.Error("An error occured whilst performing the operation.");
        }
    }
}