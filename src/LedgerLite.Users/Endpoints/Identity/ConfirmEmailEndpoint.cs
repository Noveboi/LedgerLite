
using System.Text;
using FastEndpoints;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace LedgerLite.Users.Endpoints.Identity;

internal sealed class ConfirmEmailEndpoint(UserManager<User> userManager) : EndpointWithoutRequest
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/confirmEmail");
        Description(b => b.WithName("confirmEmail"));
        Group<IdentityEndpointGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Query<string>("userId", isRequired: true)!;
        var code = Query<string>("code", isRequired: true)!;
        var changedEmail = Query<string>("changedEmail", isRequired: true)!;

        if (ValidationFailed || await userManager.FindByIdAsync(userId) is not { } user)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        IdentityResult result;

        if (string.IsNullOrEmpty(changedEmail))
        {
            result = await userManager.ConfirmEmailAsync(user, code);
        }
        else
        {
            result = await userManager.ChangeEmailAsync(user, changedEmail, code);

            if (result.Succeeded && user.IsUsingEmailAsUsername())
            {
                result = await userManager.SetUserNameAsync(user, changedEmail);
            }
        }

        if (!result.Succeeded)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        await SendStringAsync("Thank you for confirming your email.", cancellation: ct);
    }
}