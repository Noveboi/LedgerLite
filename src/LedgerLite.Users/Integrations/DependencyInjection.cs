using LedgerLite.Users.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Users.Integrations;

internal static class DependencyInjection
{
    public static IServiceCollection AddUsersIntegrations(this IServiceCollection services) => 
        services
            .AddScoped<IOrganizationRequests, OrganizationRequests>()
            .AddScoped<IUserRequests, UserRequests>();
}