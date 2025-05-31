using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using FastEndpoints;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;

namespace LedgerLite.Users.Endpoints.Identity;

internal sealed class IdentityEndpointGroup : Group
{
    public IdentityEndpointGroup()
    {
        Configure(routePrefix: "", ep: _ => { });
    }

    public static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription)
    {
        return TypedResults.ValidationProblem(errors: new Dictionary<string, string[]>
        {
            { errorCode, [errorDescription] }
        });
    }

    public static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(condition: !result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(capacity: 1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(key: error.Code, value: out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(sourceArray: descriptions, destinationArray: newDescriptions, length: descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[key: error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errors: errorDictionary);
    }

    public static async Task SendConfirmationEmailAsync(
        User user,
        UserManager<User> userManager,
        IEmailSender<User> emailSender,
        LinkGenerator linkGenerator,
        HttpContext context,
        string email,
        bool isChange = false)
    {
        var code = isChange
            ? await userManager.GenerateChangeEmailTokenAsync(user: user, newEmail: email)
            : await userManager.GenerateEmailConfirmationTokenAsync(user: user);
        code = WebEncoders.Base64UrlEncode(input: Encoding.UTF8.GetBytes(s: code));

        var userId = await userManager.GetUserIdAsync(user: user);
        var routeValues = new RouteValueDictionary
        {
            [key: "userId"] = userId,
            [key: "code"] = code
        };

        if (isChange)
            // This is validated by the /confirmEmail endpoint on change.
            routeValues.Add(key: "changedEmail", value: email);

        const string confirmEmailEndpointName = "confirmEmail";
        var confirmEmailUrl = linkGenerator.GetUriByName(httpContext: context, endpointName: confirmEmailEndpointName,
                                  values: routeValues)
                              ?? throw new NotSupportedException(
                                  message: $"Could not find endpoint named '{confirmEmailEndpointName}'.");

        await emailSender.SendConfirmationLinkAsync(user: user, email: email,
            confirmationLink: HtmlEncoder.Default.Encode(value: confirmEmailUrl));
    }
}