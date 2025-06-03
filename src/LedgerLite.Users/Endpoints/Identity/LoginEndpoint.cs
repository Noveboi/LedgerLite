using FastEndpoints;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace LedgerLite.Users.Endpoints.Identity;

internal sealed class LoginEndpoint(SignInManager<User> signInManager)
    : Endpoint<LoginRequest, AccessTokenResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/login");
        Group<IdentityEndpointGroup>();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var useCookies = Query<bool?>(paramName: "useCookies", isRequired: false);
        var useSessionCookies = Query<bool?>(paramName: "useSessionCookies", isRequired: false);

        var useCookieScheme = useCookies == true || useSessionCookies == true;
        var isPersistent = useCookies == true && useSessionCookies != true;
        signInManager.AuthenticationScheme = useCookieScheme
            ? IdentityConstants.ApplicationScheme
            : IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(userName: req.Email, password: req.Password,
            isPersistent: isPersistent, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(value: req.TwoFactorCode))
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(code: req.TwoFactorCode,
                    isPersistent: isPersistent,
                    rememberClient: isPersistent);
            else if (!string.IsNullOrEmpty(value: req.TwoFactorRecoveryCode))
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode: req.TwoFactorRecoveryCode);
        }

        if (!result.Succeeded)
            await SendResultAsync(
                TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized));
    }
}