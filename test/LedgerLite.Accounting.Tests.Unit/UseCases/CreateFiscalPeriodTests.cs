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
        _unitOfWork.ConfigureForTests(o => o.MockFiscalPeriodRepository(repo: _repository));
        _sut = new FiscalPeriodService(unitOfWork: _unitOfWork);
    }

    [Fact]
    public async Task Invalid_WhenCreateRequestIsInvalid()
    {
        var request = GetRequest(orgId: OrganizationId) with { StartDate = DateOnly.MaxValue };

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ShouldHaveError(
            FiscalPeriodErrors.StartIsAfterEnd(start: request.StartDate, end: request.EndDate));
        await _unitOfWork.AssertThatNoActionWasTaken();
    }

    [Fact]
    public async Task AddToFiscalPeriodRepository()
    {
        var request = GetRequest(orgId: OrganizationId);

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        _repository.Received(requiredNumberOfCalls: 1).Add(Arg.Is<FiscalPeriod>(f =>
            f.OrganizationId == request.OrganizationId &&
            f.StartDate == request.StartDate &&
            f.EndDate == request.EndDate));
    }

    [Fact]
    public async Task SaveChangesWhenSuccessful()
    {
        var request = GetRequest(orgId: OrganizationId);

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        await _unitOfWork.Received(requiredNumberOfCalls: 1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnValidFiscalPeriod_WhenSuccessful()
    {
        var request = GetRequest(orgId: OrganizationId);

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        var period = result.Value;
        period.OrganizationId.ShouldBe(expected: OrganizationId);
        period.StartDate.ShouldBe(expected: request.StartDate);
        period.EndDate.ShouldBe(expected: request.EndDate);
        period.ClosedAtUtc.ShouldBeNull();
        period.IsClosed.ShouldBeFalse();
    }

    [Fact]
    public async Task Invalid_WhenOverlappingWithExistingFiscalPeriod()
    {
        var overlap = ConfigureExistingPeriod(new DateOnly(year: 2024, month: 9, day: 1),
            new DateOnly(year: 2025, month: 9, day: 1));
        var request = GetRequest(orgId: OrganizationId);

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ShouldHaveError(FiscalPeriodErrors.OverlappingPeriods(
            a: overlap.Range,
            new DateRange(Start: request.StartDate, End: request.EndDate)));
    }

    [Fact]
    public async Task Invalid_WhenPeriodWithSameName_AlreadyExists_ForOrganization()
    {
        var request = GetRequest(orgId: OrganizationId);
        _repository.NameExistsForOrganizationAsync(organizationId: request.OrganizationId, name: request.Name,
                Arg.Any<CancellationToken>())
            .Returns(returnThis: true);

        var result = await _sut.CreateAsync(request: request, token: CancellationToken.None);

        result.Status.ShouldBe(expected: ResultStatus.Invalid);
        result.ShouldHaveError(FiscalPeriodErrors.PeriodWithSameName(name: request.Name));
    }

    private static CreateFiscalPeriodRequest GetRequest(Guid orgId)
    {
        return new CreateFiscalPeriodRequest(
            OrganizationId: orgId,
            new DateOnly(year: 2024, month: 1, day: 1),
            new DateOnly(year: 2025, month: 1, day: 1),
            Name: "Alright!");
    }

    private FiscalPeriod ConfigureExistingPeriod(DateOnly startDate, DateOnly endDate)
    {
        var period = FakeFiscalPeriods.Get(o => o
            .StartingAt(start: startDate)
            .EndingAt(end: endDate)
            .WithOrganization(id: OrganizationId));

        _repository
            .FindOverlappingPeriodAsync(organizationId: OrganizationId, Arg.Any<DateOnly>(),
                Arg.Any<DateOnly>(),
                Arg.Any<CancellationToken>())
            .Returns(returnThis: period);

        return period;
    }
}