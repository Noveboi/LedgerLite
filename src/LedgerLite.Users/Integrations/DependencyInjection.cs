using LedgerLite.Users.Contracts;
using LedgerLite.Users.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerLite.Users.Integrations;

internal static class DependencyInjection
{
    public static IServiceCollection AddUsersIntegrations(this IServiceCollection services)
    {
        return services
            .AddHostedService<StartupService>()
            .AddScoped<IOrganizationRequests, OrganizationRequests>()
            .AddScoped<IUserRequests, UserRequests>();
    }
}