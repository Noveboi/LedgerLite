using FastEndpoints;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed record GetOrganizationRequest([property: RouteParam] Guid Id);

internal sealed class GetOrganizationEndpoint : Endpoint<GetOrganizationRequest, OrganizationDto>
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