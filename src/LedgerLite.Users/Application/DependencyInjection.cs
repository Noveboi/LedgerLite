using LedgerLite.Users.Application.Organizations;
using LedgerLite.Users.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Users.Application;

internal static class DependencyInjection
{
    public static IServiceCollection AddUsersApplication(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IOrganizationService, OrganizationService>();
    }
}