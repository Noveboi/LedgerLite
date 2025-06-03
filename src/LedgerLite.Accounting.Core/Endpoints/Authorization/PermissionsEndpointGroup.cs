using FastEndpoints;
using Microsoft.AspNetCore.Builder;

namespace LedgerLite.Accounting.Core.Endpoints.Authorization;

internal sealed class PermissionsEndpointGroup : Group
{
    public PermissionsEndpointGroup()
    {
        Configure(routePrefix: "/permissions",
            ep =>
            {
                ep.Description(x => x.WithGroupName(endpointGroupName: "Permissions Documentation"));
            });
    }
}