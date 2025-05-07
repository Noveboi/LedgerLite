using FastEndpoints;
using LedgerLite.Users.Endpoints.Organizations.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record GetOrganizationRequest([property: FromRoute] Guid Id);

internal sealed class GetOrganizationEndpoint : Endpoint<GetOrganizationRequest, OrganizationResponseDto>
{
    public override void Configure()
    {
        Get("{id:guid}");
        Group<OrganizationEndpointGroup>();
    }

    public override Task HandleAsync(GetOrganizationRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}