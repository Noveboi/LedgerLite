using FastEndpoints;
using LedgerLite.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Users.Endpoints.Identity;

internal sealed record LedgerLiteRegisterRequest(
    string Email,
    string Password,
    string? Username,
    string? FirstName,
    string? LastName);

internal sealed class RegisterEndpoint(IServiceProvider sp) : Endpoint<LedgerLiteRegisterRequest>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("/register");
        Group<IdentityEndpointGroup>();
    }

    public override async Task HandleAsync(LedgerLiteRegisterRequest req, CancellationToken ct)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();

        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException($"LedgerLite requires a user store with email support.");
        }

        var userStore = sp.GetRequiredService<IUserStore<User>>();
        var emailStore = (IUserEmailStore<User>)userStore;
            
        var email = req.Email;

        if (string.IsNullOrEmpty(email))
        {
            await SendResultAsync(IdentityEndpointGroup
                .CreateValidationProblem(
                    IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email))));
        }

        var user = new User();
        var username = req.Username ?? req.Email;
        user.FirstName = req.FirstName;
        user.LastName = req.LastName;
        
        await userStore.SetUserNameAsync(user, username, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, req.Password);

        if (!result.Succeeded)
        {
            await SendResultAsync(IdentityEndpointGroup.CreateValidationProblem(result));
        }

        await IdentityEndpointGroup.SendConfirmationEmailAsync(
            user, 
            userManager,
            sp.GetRequiredService<IEmailSender<User>>(),
            sp.GetRequiredService<LinkGenerator>(),
            HttpContext,
            email);

        await SendOkAsync(ct);
    }
}