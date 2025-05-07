using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.Users.Application.Organizations;
using LedgerLite.Users.Application.Organizations.Requests;
using LedgerLite.Users.Domain.Organizations;
using LedgerLite.Users.Endpoints.Organizations.Dto;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record CreateOrganizationRequestDto(string Name);

internal sealed class CreateOrganizationEndpoint(IOrganizationService service) 
    : Endpoint<CreateOrganizationRequestDto, OrganizationResponseDto>
{
    public override void Configure()
    {
        Post("");
        Group<OrganizationEndpointGroup>();
    }

    public override async Task HandleAsync(CreateOrganizationRequestDto req, CancellationToken ct)
    {
        var request = MapRequest(req);
        var result = await service.CreateAsync(request, ct);
        if (!result.IsSuccess)
        {
            await SendResultAsync(result.ToMinimalApiResult());
            return;
        }

        var organization = result.Value;

        await SendCreatedAtAsync<GetOrganizationEndpoint>(
            routeValues: new { organization.Id },
            responseBody: MapResponse(organization),
            cancellation: ct);
    }

    private CreateOrganizationRequest MapRequest(CreateOrganizationRequestDto req) =>
        new(Name: req.Name);

    private OrganizationResponseDto MapResponse(Organization organization) =>
        new(Id: organization.Id, Name: organization.Name);
}