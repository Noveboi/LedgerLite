using Ardalis.Result;
using LedgerLite.Accounting.Core.Domain.Periods;
using LedgerLite.Accounting.Core.Infrastructure;
using LedgerLite.Accounting.Core.Infrastructure.Repositories;
using LedgerLite.SharedKernel.Models;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Accounting.Core.Application.FiscalPeriods;

internal sealed record CreateFiscalPeriodRequest(Guid OrganizationId, DateOnly StartDate, DateOnly EndDate, string Name);

internal interface IFiscalPeriodService
{
    Task<Result<FiscalPeriod>> CreateAsync(CreateFiscalPeriodRequest request, CancellationToken token);
}

internal sealed class FiscalPeriodService(IAccountingUnitOfWork unitOfWork) : IFiscalPeriodService
{
    private readonly IFiscalPeriodRepository _repository = unitOfWork.FiscalPeriodRepository;
    
    public async Task<Result<FiscalPeriod>> CreateAsync(CreateFiscalPeriodRequest request, CancellationToken token) =>
        await EnsurePeriodDoesNotOverlapWithAnother(request, token)
            .BindAsync(async org => await _repository.NameExistsForOrganizationAsync(request.OrganizationId, request.Name, token)
                ? Result.Invalid(FiscalPeriodErrors.PeriodWithSameName(request.Name))
                : Result.Success(org))
            .BindAsync(_ => FiscalPeriod.Create(
                organizationId: request.OrganizationId,
                startDate: request.StartDate,
                endDate: request.EndDate,
                name: request.Name))
            .BindAsync(period =>
            {
                _repository.Add(period);
                return Result.Success(period);
            })
            .BindAsync(period => unitOfWork.SaveChangesAsync(token).MapAsync(() => period));

    private async Task<Result<OrganizationDto>> EnsurePeriodDoesNotOverlapWithAnother(
        CreateFiscalPeriodRequest request,
        CancellationToken token)
    {
        var overlappingPeriod = await _repository.FindOverlappingPeriodAsync(
            request.OrganizationId,
            request.StartDate,
            request.EndDate, 
            token);

        return overlappingPeriod is null
            ? Result.Success()
            : Result.Invalid(FiscalPeriodErrors.OverlappingPeriods(
                overlappingPeriod.Range,
                new DateRange(request.StartDate, request.EndDate)));
    }
}