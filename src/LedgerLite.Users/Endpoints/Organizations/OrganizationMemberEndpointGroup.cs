using FastEndpoints;
using LedgerLite.Users.Authorization;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed class OrganizationMemberEndpointGroup : SubGroup<OrganizationEndpointGroup>
{
    public OrganizationMemberEndpointGroup()
    {
        Configure("{organizationId:guid}/members", ep => ep.Policy(x => x.RequireOrganizationAdministrator()));
    }
}