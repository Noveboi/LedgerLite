using FastEndpoints;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Serilog;

namespace LedgerLite.Users.Endpoints.Identity;

internal sealed class LoginEndpoint(SignInManager<User> signInManager) : Endpoint<LoginRequest, AccessTokenResponse>
{
    private readonly ILogger _log = Log.ForContext<LoginEndpoint>();
    
    public override void Configure()
    {
        AllowAnonymous();
        Post("/login");
        Group<IdentityEndpointGroup>();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var useCookies = Query<bool?>("useCookies", isRequired: false);
        var useSessionCookies = Query<bool?>("useSessionCookies", isRequired: false);
        
        var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
        var isPersistent = (useCookies == true) && (useSessionCookies != true);
        signInManager.AuthenticationScheme = useCookieScheme 
            ? IdentityConstants.ApplicationScheme 
            : IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(req.Email, req.Password, isPersistent, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(req.TwoFactorCode))
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(req.TwoFactorCode, isPersistent, rememberClient: isPersistent);
            }
            else if (!string.IsNullOrEmpty(req.TwoFactorRecoveryCode))
            {
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(req.TwoFactorRecoveryCode);
            }
        }

        if (!result.Succeeded)
        {
            await SendResultAsync(TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized));
        }
        
        // The signInManager already produced the needed response in the form of a cookie or bearer token.
    }
}