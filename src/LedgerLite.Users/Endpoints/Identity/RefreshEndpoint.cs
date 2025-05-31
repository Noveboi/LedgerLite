using FastEndpoints;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;

namespace LedgerLite.Users.Endpoints.Identity;

internal sealed class RefreshEndpoint(
    TimeProvider timeProvider,
    IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
    SignInManager<User> signInManager)
    : Endpoint<RefreshRequest, AccessTokenResponse>
{
    public override void Configure()
    {
        Post("/refresh");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshRequest req, CancellationToken ct)
    {
        var refreshTokenProtector = bearerTokenOptions.Get(name: IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(protectedText: req.RefreshToken);

        // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            timeProvider.GetUtcNow() >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync(principal: refreshTicket.Principal) is not { } user)

        {
            await SendResultAsync(result: TypedResults.Challenge());
            return;
        }

        var newPrincipal = await signInManager.CreateUserPrincipalAsync(user: user);
        await SendResultAsync(result: TypedResults.SignIn(principal: newPrincipal,
            authenticationScheme: IdentityConstants.BearerScheme));
    }
}