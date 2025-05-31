using FastEndpoints;
using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Infrastructure.Repositories;
using LedgerLite.Users.Integrations.Conversions;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed class GetOrganizationsEndpoint(IOrganizationRepository organizationRepository)
    : EndpointWithoutRequest<IEnumerable<OrganizationDto>>
{
    public override void Configure()
    {
        Get("");
        Group<OrganizationEndpointGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var organizations = await organizationRepository.GetAllAsync(ct);
        await SendAsync(organizations.Select(x => x.ToDto()), cancellation: ct);
    }
}