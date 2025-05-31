using FastEndpoints;
using LedgerLite.Accounting.Core.Authorization;
using LedgerLite.Accounting.Core.Endpoints.Authorization.Dto;

namespace LedgerLite.Accounting.Core.Endpoints.Authorization;

internal sealed class ModifyPermissionsEndpoint : EndpointWithoutRequest<PermissionsDocumentationResponse>
{
    public override void Configure()
    {
        Get("modify");
        Group<PermissionsEndpointGroup>();
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        var response = new PermissionsDocumentationResponse(
            "Modify",
            ModifyPolicy.AllowedRoles);

        return SendAsync(response, cancellation: ct);
    }
}