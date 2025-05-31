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
        Description(builder: b => b.WithName(endpointName: "confirmEmail"));
        Group<IdentityEndpointGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = Query<string>(paramName: "userId", isRequired: true)!;
        var code = Query<string>(paramName: "code", isRequired: true)!;
        var changedEmail = Query<string>(paramName: "changedEmail", isRequired: true)!;

        if (ValidationFailed || await userManager.FindByIdAsync(userId: userId) is not { } user)
        {
            await SendUnauthorizedAsync(cancellation: ct);
            return;
        }

        try
        {
            code = Encoding.UTF8.GetString(bytes: WebEncoders.Base64UrlDecode(input: code));
        }
        catch (FormatException)
        {
            await SendUnauthorizedAsync(cancellation: ct);
            return;
        }

        IdentityResult result;

        if (string.IsNullOrEmpty(value: changedEmail))
        {
            result = await userManager.ConfirmEmailAsync(user: user, token: code);
        }
        else
        {
            result = await userManager.ChangeEmailAsync(user: user, newEmail: changedEmail, token: code);

            if (result.Succeeded && user.IsUsingEmailAsUsername())
                result = await userManager.SetUserNameAsync(user: user, userName: changedEmail);
        }

        if (!result.Succeeded)
        {
            await SendUnauthorizedAsync(cancellation: ct);
            return;
        }

        await SendStringAsync(content: "Thank you for confirming your email.", cancellation: ct);
    }
}