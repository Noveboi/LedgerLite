using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.JournalEntries;
using LedgerLite.Accounting.Core.Application.JournalEntries.Requests;
using LedgerLite.Accounting.Core.Domain.JournalEntries;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.Accounting.Tests.Unit.Utilities;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.UseCases;

public class RecordStandardTransactionTests
{
    private static readonly CreateJournalEntryLineRequest LineRequest = new(Guid.NewGuid(), 10);
    private readonly IJournalEntryRepository _repository = Substitute.For<IJournalEntryRepository>();
    private readonly TransactionRecordingService _sut;
    private readonly IAccountingUnitOfWork _unitOfWork = Substitute.For<IAccountingUnitOfWork>();

    public RecordStandardTransactionTests()
    {
        _unitOfWork.ConfigureForTests(o => o.MockJournalEntryRepository(_repository));
        _sut = new TransactionRecordingService(_unitOfWork);
    }

    [Fact]
    public async Task DoNotSaveChanges_WhenJournalEntryCreationIsInvalid()
    {
        var invalidRequest = Request("");
        await _sut.RecordStandardEntryAsync(invalidRequest, CancellationToken.None);
        await _unitOfWork.AssertThatNoActionWasTaken();
    }

    [Fact]
    public async Task AddToRepository_WhenCreationSuccessful()
    {
        var request = SuccessfulRequest();
        await _sut.RecordStandardEntryAsync(request, CancellationToken.None);
        _repository.Received(1).Add(Arg.Any<JournalEntry>());
    }

    [Fact]
    public async Task SaveChanges_WhenCreationSuccessful()
    {
        var request = SuccessfulRequest();
        await _sut.RecordStandardEntryAsync(request, CancellationToken.None);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnValidJournalEntry_WhenCreationAndSaveSuccessful()
    {
        var request = SuccessfulRequest();

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

    [Fact]
    public async Task ReturnInvalid_WhenFiscalPeriodDoesNotExist()
    {
        var request = Request("123");
        var result = await _sut.RecordStandardEntryAsync(request, CancellationToken.None);
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(TransactionRecordingErrors.FiscalPeriodNotFound(Guid.Empty));
    }

    private FiscalPeriod ConfigureFiscalPeriod(FiscalPeriod? period = null)
    {
        period ??= FakeFiscalPeriods.Get();

        _unitOfWork.FiscalPeriodRepository
            .GetByIdAsync(period.Id, Arg.Any<CancellationToken>())
            .Returns(period);

        return period;
    }

    private RecordStandardEntryRequest SuccessfulRequest()
    {
        var period = ConfigureFiscalPeriod();
        return Request(
            "Cool!",
            userId: Guid.NewGuid(),
            periodId: period.Id);
    }

    private static RecordStandardEntryRequest Request(
        string referenceNumber,
        CreateJournalEntryLineRequest? creditRequest = null,
        CreateJournalEntryLineRequest? debitRequest = null,
        Guid? userId = null,
        Guid? periodId = null)
    {
        return new RecordStandardEntryRequest(
            referenceNumber,
            DateOnly.FromDateTime(DateTime.Today),
            "",
            creditRequest ?? LineRequest,
            debitRequest ?? LineRequest,
            userId.GetValueOrDefault(),
            periodId.GetValueOrDefault());
    }
}