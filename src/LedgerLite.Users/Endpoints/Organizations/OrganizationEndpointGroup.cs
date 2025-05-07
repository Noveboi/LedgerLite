using FastEndpoints;

namespace LedgerLite.Users.Endpoints.Organizations;

internal sealed class OrganizationEndpointGroup : Group
{
    public OrganizationEndpointGroup()
    {
        Configure("/organizations", static _ => { });
    }
}