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
    private static readonly CreateJournalEntryLineRequest LineRequest = new(Guid.NewGuid(), Amount: 10);
    private readonly IJournalEntryRepository _repository = Substitute.For<IJournalEntryRepository>();
    private readonly TransactionRecordingService _sut;
    private readonly IAccountingUnitOfWork _unitOfWork = Substitute.For<IAccountingUnitOfWork>();

    public RecordStandardTransactionTests()
    {
        _unitOfWork.ConfigureForTests(o => o.MockJournalEntryRepository(repo: _repository));
        _sut = new TransactionRecordingService(unitOfWork: _unitOfWork);
    }

    [Fact]
    public async Task DoNotSaveChanges_WhenJournalEntryCreationIsInvalid()
    {
        var invalidRequest = Request(referenceNumber: "");
        await _sut.RecordStandardEntryAsync(req: invalidRequest, ct: CancellationToken.None);
        await _unitOfWork.AssertThatNoActionWasTaken();
    }

    [Fact]
    public async Task AddToRepository_WhenCreationSuccessful()
    {
        var request = SuccessfulRequest();
        await _sut.RecordStandardEntryAsync(req: request, ct: CancellationToken.None);
        _repository.Received(requiredNumberOfCalls: 1).Add(Arg.Any<JournalEntry>());
    }

    [Fact]
    public async Task SaveChanges_WhenCreationSuccessful()
    {
        var request = SuccessfulRequest();
        await _sut.RecordStandardEntryAsync(req: request, ct: CancellationToken.None);
        await _unitOfWork.Received(requiredNumberOfCalls: 1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnValidJournalEntry_WhenCreationAndSaveSuccessful()
    {
        var request = SuccessfulRequest();

        var result = await _sut.RecordStandardEntryAsync(req: request, ct: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        result.Value.Status.ShouldBe(expected: JournalEntryStatus.Editable);
        result.Value.Lines.Count.ShouldBe(expected: 2);
        result.Value.Type.ShouldBe(expected: JournalEntryType.Standard);
    }

    [Fact]
    public async Task ReturnInvalid_WhenCreationNotSuccessful()
    {
        var invalidRequest = Request(referenceNumber: "");
        var result = await _sut.RecordStandardEntryAsync(req: invalidRequest, ct: CancellationToken.None);
        result.Status.ShouldBe(expected: ResultStatus.Invalid);
    }

    [Fact]
    public async Task ReturnInvalid_WhenFiscalPeriodDoesNotExist()
    {
        var request = Request(referenceNumber: "123");
        var result = await _sut.RecordStandardEntryAsync(req: request, ct: CancellationToken.None);
        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ShouldHaveError(TransactionRecordingErrors.FiscalPeriodNotFound(periodId: Guid.Empty));
    }

    private FiscalPeriod ConfigureFiscalPeriod(FiscalPeriod? period = null)
    {
        period ??= FakeFiscalPeriods.Get();

        _unitOfWork.FiscalPeriodRepository
            .GetByIdAsync(id: period.Id, Arg.Any<CancellationToken>())
            .Returns(returnThis: period);

        return period;
    }

    private RecordStandardEntryRequest SuccessfulRequest()
    {
        var period = ConfigureFiscalPeriod();
        return Request(
            referenceNumber: "Cool!",
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
            ReferenceNumber: referenceNumber,
            DateOnly.FromDateTime(dateTime: DateTime.Today),
            Description: "",
            creditRequest ?? LineRequest,
            debitRequest ?? LineRequest,
            userId.GetValueOrDefault(),
            periodId.GetValueOrDefault());
    }
}