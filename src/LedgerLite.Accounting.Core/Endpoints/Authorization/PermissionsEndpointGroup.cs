using FastEndpoints;
using Microsoft.AspNetCore.Builder;

namespace LedgerLite.Accounting.Core.Endpoints.Authorization;

internal sealed class PermissionsEndpointGroup : Group
{
    public PermissionsEndpointGroup()
    {
        Configure("/permissions", ep =>
        {
            ep.Description(x => x.WithGroupName("Permissions Documentation"));
        });
    }
}