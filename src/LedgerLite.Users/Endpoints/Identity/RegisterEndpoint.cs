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
            throw new NotSupportedException(message: "LedgerLite requires a user store with email support.");

        var userStore = sp.GetRequiredService<IUserStore<User>>();
        var emailStore = (IUserEmailStore<User>)userStore;

        var email = req.Email;

        if (string.IsNullOrEmpty(value: email))
            await SendResultAsync(IdentityEndpointGroup
                .CreateValidationProblem(
                    IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email: email))));

        var user = new User();
        var username = req.Username ?? req.Email;
        user.FirstName = req.FirstName;
        user.LastName = req.LastName;

        await userStore.SetUserNameAsync(user: user, userName: username, cancellationToken: CancellationToken.None);
        await emailStore.SetEmailAsync(user: user, email: email, cancellationToken: CancellationToken.None);
        var result = await userManager.CreateAsync(user: user, password: req.Password);

        if (!result.Succeeded)
            await SendResultAsync(IdentityEndpointGroup.CreateValidationProblem(result: result));

        await IdentityEndpointGroup.SendConfirmationEmailAsync(
            user: user,
            userManager: userManager,
            sp.GetRequiredService<IEmailSender<User>>(),
            sp.GetRequiredService<LinkGenerator>(),
            context: HttpContext,
            email: email);

        await SendOkAsync(cancellation: ct);
    }
}