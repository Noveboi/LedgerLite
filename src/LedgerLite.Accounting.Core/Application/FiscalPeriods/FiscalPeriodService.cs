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
            .BindAsync(bindFunc: async org =>
                await _repository.NameExistsForOrganizationAsync(organizationId: request.OrganizationId,
                    name: request.Name, token: token)
                    ? Result.Invalid(validationError: FiscalPeriodErrors.PeriodWithSameName(name: request.Name))
                    : Result.Success(value: org))
            .BindAsync(bindFunc: _ => FiscalPeriod.Create(
                organizationId: request.OrganizationId,
                startDate: request.StartDate,
                endDate: request.EndDate,
                name: request.Name))
            .BindAsync(bindFunc: period =>
            {
                _repository.Add(period: period);
                return Result.Success(value: period);
            })
            .BindAsync(bindFunc: period => unitOfWork.SaveChangesAsync(token: token).MapAsync(func: () => period));
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
            : Result.Invalid(validationError: FiscalPeriodErrors.OverlappingPeriods(
                a: overlappingPeriod.Range,
                b: new DateRange(Start: request.StartDate, End: request.EndDate)));
    }
}