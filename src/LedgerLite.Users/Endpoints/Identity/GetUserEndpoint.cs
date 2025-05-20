using Ardalis.Result.AspNetCore;
using FastEndpoints;
using LedgerLite.SharedKernel.Identity;
using LedgerLite.Users.Contracts;
using LedgerLite.Users.Contracts.Models;

namespace LedgerLite.Users.Endpoints.Identity;

internal sealed record GetUserRequest([property: FromClaim(LedgerClaims.UserId)] Guid UserId);

internal sealed class GetUserEndpoint(IUserRequests requests) : Endpoint<GetUserRequest, UserDto>
{
    public override void Configure()
    {
        Get("/me");
        Group<IdentityEndpointGroup>();
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        var userResult = await requests.GetUserByIdAsync(req.UserId, ct);
        await SendResultAsync(userResult.ToMinimalApiResult());
    }
}