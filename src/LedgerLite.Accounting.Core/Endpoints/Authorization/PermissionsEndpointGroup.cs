using FastEndpoints;
using Microsoft.AspNetCore.Builder;

namespace LedgerLite.Accounting.Core.Endpoints.Authorization;

internal sealed class PermissionsEndpointGroup : Group
{
    public PermissionsEndpointGroup()
    {
        Configure(routePrefix: "/permissions",
            ep: ep =>
            {
                ep.Description(builder: x => x.WithGroupName(endpointGroupName: "Permissions Documentation"));
            });
    }
}