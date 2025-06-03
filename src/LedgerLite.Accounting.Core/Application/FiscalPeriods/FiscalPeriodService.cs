using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Models;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Accounting.Core.Application.FiscalPeriods;

internal sealed record CreateFiscalPeriodRequest(
    Guid OrganizationId,
    DateOnly StartDate,
    DateOnly EndDate,
    string Name);

internal interface IFiscalPeriodService
{
    Task<Result<FiscalPeriod>> CreateAsync(CreateFiscalPeriodRequest request, CancellationToken token);
}

internal sealed class FiscalPeriodService(IAccountingUnitOfWork unitOfWork) : IFiscalPeriodService
{
    private readonly IFiscalPeriodRepository _repository = unitOfWork.FiscalPeriodRepository;

    public async Task<Result<FiscalPeriod>> CreateAsync(CreateFiscalPeriodRequest request, CancellationToken token)
    {
        return await EnsurePeriodDoesNotOverlapWithAnother(request: request, token: token)
            .BindAsync(async org =>
                await _repository.NameExistsForOrganizationAsync(organizationId: request.OrganizationId,
                    name: request.Name, token: token)
                    ? Result.Invalid(FiscalPeriodErrors.PeriodWithSameName(name: request.Name))
                    : Result.Success(value: org))
            .BindAsync(_ => FiscalPeriod.Create(
                organizationId: request.OrganizationId,
                startDate: request.StartDate,
                endDate: request.EndDate,
                name: request.Name))
            .BindAsync(period =>
            {
                _repository.Add(period: period);
                return Result.Success(value: period);
            })
            .BindAsync(period => unitOfWork.SaveChangesAsync(token: token).MapAsync(() => period));
    }

    private async Task<Result<OrganizationDto>> EnsurePeriodDoesNotOverlapWithAnother(
        CreateFiscalPeriodRequest request,
        CancellationToken token)
    {
        var overlappingPeriod = await _repository.FindOverlappingPeriodAsync(
            organizationId: request.OrganizationId,
            startDate: request.StartDate,
            endDate: request.EndDate,
            token: token);

        return overlappingPeriod is null
            ? Result.Success()
            : Result.Invalid(FiscalPeriodErrors.OverlappingPeriods(
                a: overlappingPeriod.Range,
                new DateRange(Start: request.StartDate, End: request.EndDate)));
    }
}