using FastEndpoints;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Contracts.Models;
using LedgerLite.Users.Domain;
using LedgerLite.Users.Integrations.Conversions;
using Microsoft.AspNetCore.Identity;

namespace LedgerLite.Users.Endpoints.Identity;

internal sealed record GetUserRequest([property: FromClaim(LedgerClaims.UserId)] Guid UserId);

internal sealed class GetUserEndpoint(UserManager<User> userManager) : Endpoint<GetUserRequest, UserDto>
{
    public override void Configure()
    {
        Get("/me");
        Group<IdentityEndpointGroup>();
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        if (await userManager.FindByIdAsync(req.UserId.ToString()) is not { } user)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await SendAsync(user.ToDto(), cancellation: ct);
    }
}