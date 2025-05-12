using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Accounting.Core.Application.FiscalPeriods;
using LedgerLite.Accounting.Core.Application.UseCases;
using LedgerLite.Accounting.Core.Endpoints.FiscalPeriods.Dto;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Accounting.Core.Endpoints.FiscalPeriods;

internal sealed record CreateFiscalPeriodRequestDto(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate);

internal sealed class CreateFiscalPeriodEndpoint(
    GetOrganizationFromUserUseCase getOrganizationFromUser, 
    IFiscalPeriodService service)
    : Endpoint<CreateFiscalPeriodRequestDto, FiscalPeriodDto>
{
    public override void Configure()
    {
        Post("");
        Group<FiscalPeriodEndpointGroup>();
    }

    public override async Task HandleAsync(CreateFiscalPeriodRequestDto req, CancellationToken ct)
    {
        var organizationResult = await getOrganizationFromUser.HandleAsync(req.UserId, ct);
        if (!organizationResult.IsSuccess)
        {
            await SendResultAsync(organizationResult.ToMinimalApiResult());
            return;
        }

        var request = MapRequest(req, organizationResult.Value);
        var creationResult = await service.CreateAsync(request, ct);

        if (!creationResult.IsSuccess)
        {
            await SendResultAsync(creationResult.ToMinimalApiResult());
        }

        var period = creationResult.Value;

        await SendCreatedAtAsync<GetFiscalPeriodEndpoint>(
            routeValues: new { period.Id },
            responseBody: period.ToDto(),
            cancellation: ct);
    }

    public static CreateFiscalPeriodRequest MapRequest(CreateFiscalPeriodRequestDto dto, OrganizationDto org) =>
        new(OrganizationId: org.Id,
            StartDate: dto.StartDate,
            EndDate: dto.EndDate);
}