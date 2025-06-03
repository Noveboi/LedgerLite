using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.FiscalPeriods;
using LedgerLite.Accounting.Core.Application.UseCases;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Groups;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods;

internal sealed record CreateFiscalPeriodRequestDto(
    [property: FromClaim(claimType: LedgerClaims.UserId)]
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate,
    string Name);

internal sealed class CreateFiscalPeriodEndpoint(
    GetOrganizationFromUserUseCase getOrganizationFromUser,
    IFiscalPeriodService service)
    : Endpoint<CreateFiscalPeriodRequestDto, FiscalPeriodDto>
{
    public override void Configure()
    {
        Post("");
        Group<ModifyFiscalPeriodGroup>();
    }

    public override async Task HandleAsync(CreateFiscalPeriodRequestDto req, CancellationToken ct)
    {
        var organizationResult = await getOrganizationFromUser.HandleAsync(request: req.UserId, token: ct);
        if (!organizationResult.IsSuccess)
        {
            await SendResultAsync(organizationResult.ToMinimalApiResult());
            return;
        }

        var request = MapRequest(dto: req, org: organizationResult.Value);
        var creationResult = await service.CreateAsync(request: request, token: ct);

        if (!creationResult.IsSuccess) await SendResultAsync(creationResult.ToMinimalApiResult());

        var period = creationResult.Value;

        await SendCreatedAtAsync<GetFiscalPeriodsEndpoint>(
            routeValues: null,
            period.ToDto(),
            cancellation: ct);
    }

    public static CreateFiscalPeriodRequest MapRequest(CreateFiscalPeriodRequestDto dto, OrganizationDto org)
    {
        return new CreateFiscalPeriodRequest(OrganizationId: org.Id,
            StartDate: dto.StartDate,
            EndDate: dto.EndDate,
            Name: dto.Name);
    }
}