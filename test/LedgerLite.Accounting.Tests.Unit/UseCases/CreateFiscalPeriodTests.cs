using Ardalis.Result;
using LedgerLite.Accounting.Core.Application.FiscalPeriods;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.Accounting.Tests.Unit.Utilities;
using LedgerLite.Accounting.Tests.Unit.Utilities.Fakes;
using LedgerLite.SharedKernel.Models;
using LedgerLite.Tests.Shared;

namespace LedgerLite.Accounting.Tests.Unit.UseCases;

public class CreateFiscalPeriodTests
{
    private static readonly Guid OrganizationId = Guid.NewGuid();
    private readonly IFiscalPeriodRepository _repository = Substitute.For<IFiscalPeriodRepository>();
    private readonly FiscalPeriodService _sut;
    private readonly IAccountingUnitOfWork _unitOfWork = Substitute.For<IAccountingUnitOfWork>();

    public CreateFiscalPeriodTests()
    {
        _unitOfWork.ConfigureForTests(o => o.MockFiscalPeriodRepository(_repository));
        _sut = new FiscalPeriodService(_unitOfWork);
    }

    [Fact]
    public async Task Invalid_WhenCreateRequestIsInvalid()
    {
        var request = GetRequest(OrganizationId) with { StartDate = DateOnly.MaxValue };

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(FiscalPeriodErrors.StartIsAfterEnd(request.StartDate, request.EndDate));
        await _unitOfWork.AssertThatNoActionWasTaken();
    }

    [Fact]
    public async Task AddToFiscalPeriodRepository()
    {
        var request = GetRequest(OrganizationId);

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Ok);
        _repository.Received(1).Add(Arg.Is<FiscalPeriod>(f => f.OrganizationId == request.OrganizationId &&
                                                              f.StartDate == request.StartDate &&
                                                              f.EndDate == request.EndDate));
    }

    [Fact]
    public async Task SaveChangesWhenSuccessful()
    {
        var request = GetRequest(OrganizationId);

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Ok);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnValidFiscalPeriod_WhenSuccessful()
    {
        var request = GetRequest(OrganizationId);

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        var period = result.Value;
        period.OrganizationId.ShouldBe(OrganizationId);
        period.StartDate.ShouldBe(request.StartDate);
        period.EndDate.ShouldBe(request.EndDate);
        period.ClosedAtUtc.ShouldBeNull();
        period.IsClosed.ShouldBeFalse();
    }

    [Fact]
    public async Task Invalid_WhenOverlappingWithExistingFiscalPeriod()
    {
        var overlap = ConfigureExistingPeriod(new DateOnly(2024, 9, 1), new DateOnly(2025, 9, 1));
        var request = GetRequest(OrganizationId);

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(FiscalPeriodErrors.OverlappingPeriods(
            overlap.Range,
            new DateRange(request.StartDate, request.EndDate)));
    }

    [Fact]
    public async Task Invalid_WhenPeriodWithSameName_AlreadyExists_ForOrganization()
    {
        var request = GetRequest(OrganizationId);
        _repository.NameExistsForOrganizationAsync(request.OrganizationId, request.Name, Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ShouldHaveError(FiscalPeriodErrors.PeriodWithSameName(request.Name));
    }

    private static CreateFiscalPeriodRequest GetRequest(Guid orgId)
    {
        return new CreateFiscalPeriodRequest(
            orgId,
            new DateOnly(2024, 1, 1),
            new DateOnly(2025, 1, 1),
            "Alright!");
    }

    private FiscalPeriod ConfigureExistingPeriod(DateOnly startDate, DateOnly endDate)
    {
        var period = FakeFiscalPeriods.Get(o => o
            .StartingAt(startDate)
            .EndingAt(endDate)
            .WithOrganization(OrganizationId));

        _repository
            .FindOverlappingPeriodAsync(OrganizationId, Arg.Any<DateOnly>(), Arg.Any<DateOnly>(),
                Arg.Any<CancellationToken>())
            .Returns(period);

        return period;
    }
}