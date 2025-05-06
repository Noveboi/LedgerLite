using Ardalis.Result;
using LedgerLite.Accounting.Core.Application;
using LedgerLite.Accounting.Core.Application.JournalEntries;
using LedgerLite.Accounting.Core.Application.JournalEntries.Requests;
using LedgerLite.Accounting.Core.Domain.JournalEntries;

namespace LedgerLite.Accounting.Tests.Unit.UseCases;

public class RecordStandardTransactionTests
{
    private readonly TransactionRecordingService _sut;
    private readonly IJournalEntryRepository _repository = Substitute.For<IJournalEntryRepository>();
    private readonly IAccountingUnitOfWork _unitOfWork = Substitute.For<IAccountingUnitOfWork>();

    private static readonly CreateJournalEntryLineRequest LineRequest = new(Guid.NewGuid(), 10);

    public RecordStandardTransactionTests()
    {
        _unitOfWork.JournalEntryRepository.Returns(_repository);
        _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());
        _sut = new TransactionRecordingService(_unitOfWork);
    }

    [Fact]
    public async Task DoNotSaveChanges_WhenJournalEntryCreationIsInvalid()
    {
        var invalidRequest = Request("");
        await _sut.RecordStandardEntryAsync(invalidRequest, CancellationToken.None);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddToRepository_WhenCreationSuccessful()
    {
        var request = Request("MARK41039213231");
        await _sut.RecordStandardEntryAsync(request, CancellationToken.None);
        _repository.Received(1).Add(Arg.Any<JournalEntry>());
    }

    [Fact]
    public async Task SaveChanges_WhenCreationSuccessful()
    {
        var request = Request("ABC123");
        await _sut.RecordStandardEntryAsync(request, CancellationToken.None);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnValidJournalEntry_WhenCreationAndSaveSuccessful()
    {
        var request = Request("Hello!");

        var result = await _sut.RecordStandardEntryAsync(request, CancellationToken.None);
        
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.Status.ShouldBe(JournalEntryStatus.Editable);
        result.Value.Lines.Count.ShouldBe(2);
        result.Value.Type.ShouldBe(JournalEntryType.Standard);
    }

    [Fact]
    public async Task ReturnInvalid_WhenCreationNotSuccessful()
    {
        var invalidRequest = Request("");
        var result = await _sut.RecordStandardEntryAsync(invalidRequest, CancellationToken.None);
        result.Status.ShouldBe(ResultStatus.Invalid);
    }
    
    private static RecordStandardEntryRequest Request(
        string referenceNumber,
        CreateJournalEntryLineRequest? creditRequest = null,
        CreateJournalEntryLineRequest? debitRequest = null)
    {
        return new RecordStandardEntryRequest(
            ReferenceNumber: referenceNumber,
            OccursAtUtc: DateTime.Today,
            Description: "",
            CreditLine: creditRequest ?? LineRequest,
            DebitLine: debitRequest ?? LineRequest);
    }
}