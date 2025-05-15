using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Application.Organizations;
using LedgerLite.Users.Application.Organizations.Requests;
using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Integrations.Conversions;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record CreateOrganizationRequestDto(
    [property: FromClaim(LedgerClaims.UserId)] Guid UserId,
    string Name);

internal sealed class CreateOrganizationEndpoint(IOrganizationService organizationService) 
    : Endpoint<CreateOrganizationRequestDto, OrganizationDto>
{
    public override void Configure()
    {
        Post("");
        Group<OrganizationEndpointGroup>();
    }

    public override async Task HandleAsync(CreateOrganizationRequestDto req, CancellationToken ct)
    {
        var request = MapRequest(req);
        var createResult = await organizationService.CreateAsync(request, ct);
        if (!createResult.IsSuccess)
        {
            await SendResultAsync(createResult.ToMinimalApiResult());
            return;
        }

        var organization = createResult.Value;

        await SendCreatedAtAsync<GetOrganizationEndpoint>(
            routeValues: new { organization.Id },
            responseBody: organization.ToDto(),
            cancellation: ct);
    }

    private static CreateOrganizationRequest MapRequest(CreateOrganizationRequestDto req) =>
        new(UserId: req.UserId,
            Name: req.Name);
}